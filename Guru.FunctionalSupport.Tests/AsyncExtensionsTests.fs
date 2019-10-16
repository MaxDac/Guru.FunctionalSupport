namespace Com.Guru.FunctionalSupport.Tests

open Com.Guru.FunctionalSupport
open System.Diagnostics
open System.Threading.Tasks
open NUnit.Framework

[<TestFixture>]
type AsyncExtensionsTests() =
    [<Test>]
    member this.plainTaskAsAsyncTest() =
        let task = Task.Delay(1100)
        let taskAsAsync = Async.awaitPlainTask task
        let stopwatch = new Stopwatch()
        
        async {
            stopwatch.Start()
            let! result = taskAsAsync
            stopwatch.Stop()
            let elapsed = stopwatch.ElapsedMilliseconds
            printfn "Elapsed %d" elapsed
            elapsed > int64(1000) |> AssertF.isTrue
            return result
        } |> Async.RunSynchronously
        
    [<Test>]
    member this.asyncAsPlainTask() =
        let temp = Async.Sleep 1010
        let task = Async.asPlainTask temp
        let stopwatch = new Stopwatch()
        
        stopwatch.Start()
        task.RunSynchronously()
        stopwatch.Stop()
        
        let elapsed = stopwatch.ElapsedMilliseconds
        printfn "Elapsed: %d" elapsed
        
        elapsed > int64(1000) |> AssertF.isTrue
        
    [<Test>]
    member this.UnitTest() =
        let temp = Async.unit 10
        
        let result =
            async {
                return! temp
            } |> Async.RunSynchronously
            
        AssertF.areEqual 10 result
        
    [<Test>]
    member this.BindTest() =
        let temp = async {
            do! Async.Sleep 1000
            return 10
        }
        
        let temp2 x = async {
            do! Async.Sleep 1000
            return 10 + x
        }
        
        let bound = Async.bind temp2 temp
        
        async {
            let stopwatch = new Stopwatch()
            stopwatch.Start()
            let! result = bound
            stopwatch.Stop()
            
            let elapsed = stopwatch.ElapsedMilliseconds
            printfn "Elapsed: %d" elapsed
            
            AssertF.areEqual 20 result
            elapsed > int64(2000) |> Assert.IsTrue            
        } |> Async.RunSynchronously
        
    [<Test>]
    member this.MapTest() =
        let temp = async {
            do! Async.Sleep 1000
            return 10
        }
        
        let temp2 x = x + 10
        
        let bound = Async.map temp2 temp
        
        async {
            let stopwatch = new Stopwatch()
            stopwatch.Start()
            let! result = bound
            stopwatch.Stop()
            
            let elapsed = stopwatch.ElapsedMilliseconds
            printfn "Elapsed: %d" elapsed
            
            AssertF.areEqual 20 result
            elapsed > int64(1000) |> Assert.IsTrue            
        } |> Async.RunSynchronously
