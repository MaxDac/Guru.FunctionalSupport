namespace Com.Guru.FunctionalSupport.Tests

open Com.Guru.FunctionalSupport
open System.IO
open System.Text
open NUnit.Framework

//TODO - Complete the unit tests
[<TestFixture>]
type StreamsTests() =
    
    [<Test>]
    member this.MergeStreamTest() =
        let streamFromValue (value : string) = async {
            let stream = new MemoryStream()
            do! stream.AsyncWrite(Encoding.UTF8.GetBytes(value))
            return stream
        }
        
        let firstString = "sto cazzo"
        let secondString = "Sto gran cazzo"
        
        let streamOne = streamFromValue firstString |> Async.RunSynchronously 
        let streamTwo = streamFromValue secondString |> Async.RunSynchronously
        let resultStream = StreamF.mergeStreams streamOne streamTwo
        
        match resultStream with
        | Ok v ->
            let resultString = StreamF.readStreamAsString v
            Assert.AreEqual(firstString + secondString, resultString)
        | Error e ->
            Assert.Fail(e.ToString())
    
    [<Test>]
    member this.MergeStreamTestAsync() =
        let streamFromValue (value : string) = async {
            let stream = new MemoryStream()
            do! stream.AsyncWrite(Encoding.UTF8.GetBytes(value))
            return stream
        }
        
        let firstString = "sto cazzo"
        let secondString = "Sto gran cazzo"
        
        async {
            let! streamOne = streamFromValue firstString 
            let! streamTwo = streamFromValue secondString
            let! resultStream = StreamF.mergeStreamsAsync streamOne streamTwo
            
            match resultStream with
            | Ok v ->
                let resultString = StreamF.readStreamAsString v
                Assert.AreEqual(firstString + secondString, resultString)
            | Error e ->
                Assert.Fail(e.ToString())
        } |> Async.asPlainTask





