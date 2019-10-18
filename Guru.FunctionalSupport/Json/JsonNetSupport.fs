namespace Com.Guru.FunctionalSupport

open System
open System.Collections.Generic
open Newtonsoft.Json
open Microsoft.FSharp.Reflection

type OptionConverter() =
    inherit JsonConverter()
    
    override x.CanConvert(t) =
        let isOptionAndNotResult() =
            let genericTypeDefinition = t.GetGenericTypeDefinition()
            let returnValue = genericTypeDefinition = typedefof<option<_>> && genericTypeDefinition <> typedefof<Result<_, _>>
            returnValue
            
        if t.IsGenericType then isOptionAndNotResult()
        else false
        
    override x.WriteJson(writer, value, serializer) =
        let value = 
            if value = null then null
            else 
                let _,fields = FSharpValue.GetUnionFields(value, value.GetType())
                fields.[0]  
        serializer.Serialize(writer, value)

    override x.ReadJson(reader, t, existingValue, serializer) =        
        let innerType = t.GetGenericArguments().[0]
        let innerType = 
            if innerType.IsValueType then (typedefof<Nullable<_>>).MakeGenericType([|innerType|])
            else innerType        
        let value = serializer.Deserialize(reader, innerType)
        let cases = FSharpType.GetUnionCases(t)
        if value = null then FSharpValue.MakeUnion(cases.[0], [||])
        else FSharpValue.MakeUnion(cases.[1], [|value|])

type ListConverter() =
    inherit JsonConverter()

    override x.CanConvert(t:Type) = 
        t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<list<_>>

    override x.WriteJson(writer, value, serializer) =
        let list = value :?> System.Collections.IEnumerable |> Seq.cast
        serializer.Serialize(writer, list)

    override x.ReadJson(reader, t, _, serializer) = 
        let itemType = t.GetGenericArguments().[0]
        let collectionType = typedefof<IEnumerable<_>>.MakeGenericType(itemType)
        let collection = serializer.Deserialize(reader, collectionType) :?> IEnumerable<_>        
        let listType = typedefof<list<_>>.MakeGenericType(itemType)
        let cases = FSharpType.GetUnionCases(listType)
        let rec make = function
            | [] -> FSharpValue.MakeUnion(cases.[0], [||])
            | head::tail -> FSharpValue.MakeUnion(cases.[1], [| head; (make tail); |])                    
        make (collection |> Seq.toList)

type TupleArrayConverter() =
    inherit JsonConverter()

    override x.CanConvert(t:Type) = 
        FSharpType.IsTuple(t)

    override x.WriteJson(writer, value, serializer) =
        let values = FSharpValue.GetTupleFields(value)
        serializer.Serialize(writer, values)

    override x.ReadJson(reader, t, _, serializer) =
        let advance = reader.Read >> ignore
        let deserialize t = serializer.Deserialize(reader, t)
        let itemTypes = FSharpType.GetTupleElements(t)

        let readElements() =
            let rec read index acc =
                match reader.TokenType with
                | JsonToken.EndArray -> acc
                | _ ->
                    let value = deserialize(itemTypes.[index])
                    advance()
                    read (index + 1) (acc @ [value])
            advance()
            read 0 List.empty

        match reader.TokenType with
        | JsonToken.StartArray ->
            let values = readElements()
            FSharpValue.MakeTuple(values |> List.toArray, t)
        | _ -> failwith "invalid token"
