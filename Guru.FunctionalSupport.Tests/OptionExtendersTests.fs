namespace Com.Guru.FunctionalSupport.Tests

open Com.Guru.FunctionalSupport
open NUnit.Framework

[<TestFixture>]
type OptionExtendersTests() =
    [<Test>]
    member this.getOrElseFalseWhenTrueTest() =
        let temp = Option.unit true
        let result = Option.orElseFalse temp
        
        AssertF.isTrue result
        
    [<Test>]
    member this.getOrElseFalseWhenFalseTest() =
        let temp = Option.unit false
        let result = Option.orElseFalse temp
        
        AssertF.isFalse result
        
    [<Test>]
    member this.getOrElseFalseWhenNoneTest() =
        let temp = None
        let result = Option.orElseFalse temp
        
        AssertF.isFalse result
        
    [<Test>]
    member this.stringAsOptionWhenNullTest() =
        let temp: string = null
        let result = StringF.asOption temp
        
        result |> Option.isNone |> AssertF.isTrue
        
    [<Test>]
    member this.stringAsOptionWhenNotNullTest() =
        let temp = "not-null"
        let result = StringF.asOption temp
        
        result |> Option.isSome |> AssertF.isTrue
        result |> Option.get |> AssertF.areEqual temp
        
    [<Test>]
    member this.optionAsResultorNoneTest() =
        let temp = None
        let ex = exn "No data found"
        let asResult = Option.asResult ex temp
        
        match asResult with
        | Error e ->
            AssertF.areEqual "No data found" e.Message
        | _ ->
            AssertF.fail "It should be an error"
        
    [<Test>]
    member this.optionAsResultorSomeTest() =
        let temp = Some "value"
        let ex = exn "No data found"
        let asResult = Option.asResult ex temp
        
        match asResult with
        | Ok v ->
            AssertF.areEqual "value" v
        | _ ->
            AssertF.fail "It should be ok"
        
    [<Test>]
    member this.traverseStringOptionFirstTest() =
        let list = [ Some "string"; Some "other string" ]
        let result = list |> Option.traverseList
        
        result |> Option.isSome |> AssertF.isTrue
        
        match result |> Option.get with
        | [ el1; el2 ] ->
            AssertF.areEqual "string" el1
            AssertF.areEqual "other string" el2
        | _ ->
            AssertF.fail "The list should have had two elements"
        
    [<Test>]
    member this.traverseStringOptionSecondTest() =
        let list = [ Some "string"; None ]
        let result = list |> Option.traverseList
        
        result |> Option.isNone |> AssertF.isTrue
        
    [<Test>]
    member this.traverseStringOptionThirdTest() =
        let list = [ None; Some "string" ]
        let result = list |> Option.traverseList
        
        result |> Option.isNone |> AssertF.isTrue
        
    [<Test>]
    member this.traverseStringOptionFourthTest() =
        let list = [ None; None ]
        let result = list |> Option.traverseList
        
        result |> Option.isNone |> AssertF.isTrue
