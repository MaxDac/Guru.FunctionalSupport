namespace Com.Guru.FunctionalSupport.Tests

open System
open System.Net
open NUnit.Framework
open Newtonsoft.Json
open Com.Guru.FunctionalSupport

type DefaultError = {
    ApiErrorCode: HttpStatusCode;
    Code: string;
    [<JsonConverter(typedefof<OptionConverter>)>]
    Details: string option;
}

type Example = {
    id: string;
    [<JsonConverter(typedefof<OptionConverter>)>]
    code: string option;
    value: decimal;
    creationTime: DateTime;
}

type NestedExample = {
    id: string;
    [<JsonConverter(typedefof<OptionConverter>)>]
    code: string option;
    [<JsonConverter(typedefof<ListConverter>)>]
    firsts: Example list;
    [<JsonConverter(typedefof<OptionConverter>)>]
    second: Example option;
}

type ResultExampleInnerObject = {
    name: string;
    age: int;
}

type ResultExample = {
//        [<JsonConverter(typedefof<ResultConverter>)>]
    result: Result<ResultExampleInnerObject, DefaultError>;
}

[<TestFixture>]
type SerializationTests() =
    
    [<Test>]
    member this.SerializationTest() =
        let example = {
            id = "1";
            code = Some "21";
            value = 21.2m;
            creationTime = DateTime.Now;
        }
        
        let serialized = Json.serialize example
        let deserialized = Json.deserialize<Example> serialized
        
        let test x =
            AssertF.areEqual x example
        
        AssertF.assertResultWith test deserialized
        
    [<Test>]
    member this.SerializationTestWithNull() =
        let example = {
            id = "1";
            code = None;
            value = 21.2m;
            creationTime = DateTime.Now;
        }
            
        let serialized = Json.serialize example
        let deserialized = Json.deserialize<Example> serialized
            
        let test x =
            AssertF.areEqual x example
            
        AssertF.assertResultWith test deserialized
        
    [<Test>]
    member this.SerializationTestWithList() =
        let example = {
            id = "1";
            code = None;
            value = 21.2m;
            creationTime = DateTime.Now;
        }

        let nestedExample = {
            id = "2";
            code = Some "50";
            firsts = [ example; example; example ];
            second = Some example;
        }
            
        let serialized = Json.serialize nestedExample
        let deserialized = Json.deserialize<NestedExample> serialized
            
        let test x =
            AssertF.areEqual x nestedExample
            
        AssertF.assertResultWith test deserialized
        
    [<Test>]
    member this.SerializationTestWithListAndElementNull() =
        let example = {
            id = "1";
            code = None;
            value = 21.2m;
            creationTime = DateTime.Now;
        }

        let nestedExample = {
            id = "2";
            code = Some "50";
            firsts = [ example; example; example ];
            second = None;
        }
            
        let serialized = Json.serialize nestedExample
        let deserialized = Json.deserialize<NestedExample> serialized
            
        let test x =
            AssertF.areEqual x nestedExample
            
        AssertF.assertResultWith test deserialized
        
    [<Test>]
    member this.SerializationTestWithListWithNull() =
        let example = {
            id = "1";
            code = None;
            value = 21.2m;
            creationTime = DateTime.Now;
        }

        let nestedExample = {
            id = "2";
            code = Some "50";
            firsts = [ example; example; example ];
            second = None;
        }
            
        let serialized = Json.serialize nestedExample
        let deserialized = Json.deserialize<NestedExample> serialized
            
        let test x =
            AssertF.areEqual x nestedExample
            
        AssertF.assertResultWith test deserialized
        
    [<Test>]
    member this.SerializationTestWithEmptyList() =
        let example = {
            id = "1";
            code = None;
            value = 21.2m;
            creationTime = DateTime.Now;
        }

        let nestedExample = {
            id = "2";
            code = Some "50";
            firsts = [];
            second = None;
        }
            
        let serialized = Json.serialize nestedExample
        let deserialized = Json.deserialize<NestedExample> serialized
            
        let test x =
            AssertF.areEqual x nestedExample
            
        AssertF.assertResultWith test deserialized
    
    [<Test>]
    member this.ResultSerializationTest() =
        let example: ResultExample = {
            result = Result.Ok {
                name = "Giacomo";
                age = 11
            };
        }
        
        let serialized = Json.serialize example
        let deserialized = Json.deserialize<ResultExample> serialized
        
        let test x =
            AssertF.areEqual x example
            
        AssertF.assertResultWith test deserialized
    
    [<Test>]
    member this.ResultWithErrorSerializationTest() =
        let forException ex = {
            ApiErrorCode = HttpStatusCode.BadRequest;
            Code = "AGV001";
            Details = ex.ToString() |> Some;
        }
        
        let example: ResultExample = {
            result = Result.Error (new ArgumentNullException("Test message") |> forException)
        }
        
        let serialized = Json.serialize example
        let deserialized = Json.deserialize<ResultExample> serialized
        
        let test x =
            AssertF.areEqual x example
        
        AssertF.assertResultWith test deserialized
        

