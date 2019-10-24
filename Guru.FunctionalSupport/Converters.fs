namespace Com.Guru.FunctionalSupport

open System

module Converters =

    let parseNumber<'a>(parser: string -> bool * 'a)(s: string) =
        match (parser s) with
        | (true, i) -> Option.Some i
        | (false, _) -> Option.None

    
    let parseInt = parseNumber Int32.TryParse
        
    let parseDecimal = parseNumber Decimal.TryParse

    let parseDouble = parseNumber Double.TryParse

    let parseDateTime = parseNumber DateTime.TryParse

    let parseObjectInt(s: obj) = s.ToString() |> parseInt

    let parseObjectDecimal(s: obj) = s.ToString() |> parseDecimal

    let parseObjectDouble(s: obj) = s.ToString() |> parseDouble

    let parseObjectDateTime(s: obj) = s.ToString() |> parseDateTime
    
    let toFSharpFunc<'a, 'b>(csharpFunc: Func<'a, 'b>) =
        let internalDelegate = fun a -> csharpFunc.Invoke a
        internalDelegate
    
    let toCSharpFunc<'a, 'b>(fsharpFunc: 'a -> 'b) = new Func<'a, 'b>(fsharpFunc)

