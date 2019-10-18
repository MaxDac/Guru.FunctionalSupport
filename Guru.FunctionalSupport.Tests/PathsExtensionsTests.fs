namespace Com.Guru.FunctionalSupport.Tests

open Com.Guru.FunctionalSupport
open NUnit.Framework

[<TestFixture>]
type PathsExtensionsTests() =
    [<Test>]
    member this.combineTest() =
        let firstPath = "/usr/bin"
        let secondPath = "dotnet"
        let result = PathF.combine firstPath secondPath
        
        AssertF.areEqual "/usr/bin/dotnet" result
