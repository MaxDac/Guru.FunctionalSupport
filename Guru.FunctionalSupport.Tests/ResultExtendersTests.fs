namespace Com.Guru.FunctionalSupport.Tests

open NUnit.Framework
open Com.Guru.FunctionalSupport
open Com.Guru.FunctionalSupport.Results

[<TestFixture>]
type ResultExtendersTests() =
    
    [<Test>]
    member this.IsOkTest() =
        let ok = Ok "this is ok"
        let notOk = Error "this is the error"
        
        AssertF.isTrue ok.isOk
        AssertF.isFalse notOk.isOk
    
    [<Test>]
    member this.IsErrorTest() =
        let ok = Ok "this is ok"
        let notOk = Error "this is the error"
        
        AssertF.isFalse ok.isError
        AssertF.isTrue notOk.isError
    
    [<Test>]
    member this.GetTest() =
        let ok = Ok "this is ok"
        let notOk = Error "this is the error"
        
        ok |> Result.get |> AssertF.areEqual "this is ok"
        
        try
            notOk |> Result.get |> ignore
        with
        | e -> e |> AssertF.areEqual Results.notExistentException
        
    [<Test>]
    member this.UnitTest() =
        let temp = "this is contained"
        let result = Result.unit temp
        
        AssertF.isTrue result.isOk
        result |> Result.get |> AssertF.areEqual temp
    
    [<Test>]
    member this.MapTest() =
        let first = Ok "First value"
        let f x = sprintf "%s with second value" x
        let result = first |> Result.map f
        
        AssertF.isTrue result.isOk
        result |> Result.get |> AssertF.areEqual "First value with second value"
    
    [<Test>]
    member this.MapErrorTest() =
        let first = Error "Error text"
        let f x = sprintf "%s with second value" x
        let result = first |> Result.map f
        
        AssertF.isTrue result.isError
        
        match result with
        | Error e -> e |> AssertF.areEqual "Error text"
        | _ -> AssertF.fail "The result is not an error"
    
    [<Test>]
    member this.BindTest() =
        let first = Ok "First value"
        let f x = sprintf "%s with second value" x |> Ok
        let result = first |> Result.bind f
        
        AssertF.isTrue result.isOk
        result |> Result.get |> AssertF.areEqual "First value with second value"
    
    [<Test>]
    member this.BindFirstErrorTest() =
        let first = Error "Error text"
        let f x = sprintf "%s with second value" x |> Ok
        let result = first |> Result.bind f
        
        AssertF.isTrue result.isError
        
        match result with
        | Error e -> e |> AssertF.areEqual "Error text"
        | _ -> AssertF.fail "The result is not an error."
    
    [<Test>]
    member this.BindSecondErrorTest() =
        let first = Ok "First value"
        let f _ = Error "Error Text"
        let result = first |> Result.bind f
        
        AssertF.isTrue result.isError
        result |> Result.get |> AssertF.areEqual "First value with second value"
        
