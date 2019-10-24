namespace Com.Guru.FunctionalSupport.Tests

open Com.Guru.FunctionalSupport
open NUnit.Framework

[<TestFixture>]
type ConvertersTests() =
    [<Test>]
    member this.int32OkTest() =
        let template = "30"
        let result = template |> Converters.parseInt
        
        result |> Option.isSome |> AssertF.isTrue
        result |> Option.get |> AssertF.areEqual 30
        
    [<Test>]
    member this.int32NotOkTest() =
        let template = "Something"
        let result = template |> Converters.parseInt
        
        result |> Option.isNone |> AssertF.isTrue
        
    [<Test>]
    member this.decimalOkTest() =
        let template = "30.05"
        let result = template |> Converters.parseDecimal
        
        result |> Option.isSome |> AssertF.isTrue
        result |> Option.get |> AssertF.areEqual 30.05m
        
    [<Test>]
    member this.decimalNotOkTest() =
        let template = "Something"
        let result = template |> Converters.parseDecimal
        
        result |> Option.isNone |> AssertF.isTrue
        
    [<Test>]
    member this.doubleOkTest() =
        let template = "30.03"
        let result = template |> Converters.parseDouble
        
        result |> Option.isSome |> AssertF.isTrue
        result |> Option.get |> AssertF.areEqual 30.03
        
    [<Test>]
    member this.doubleNotOkTest() =
        let template = "Something"
        let result = template |> Converters.parseDecimal
        
        result |> Option.isNone |> AssertF.isTrue
