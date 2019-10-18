namespace Com.Guru.FunctionalSupport.Tests

open NUnit.Framework
open Com.Guru.FunctionalSupport
open Com.Guru.FunctionalSupport.ComputationExpressions

[<TestFixture>]
type ResultComputationExpressionTests() =
    [<Test>]
    member this.TestSimple() =
        let firstComputation = Ok "Computation went well"
        let secondComputation x = sprintf "%s and the secondo part as well" x |> Ok

        let result = 
            resultOf {
                let! firstPart = firstComputation
                return! secondComputation firstPart
            }

        result |> Result.isOk |> AssertF.isTrue
        result |> Result.get |> AssertF.areEqual "Computation went well and the secondo part as well"
        
    