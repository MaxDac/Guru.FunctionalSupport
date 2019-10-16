namespace Com.Guru.FunctionalSupport
open System.Threading.Tasks

module Async =
    let asPlainTask(a: Async<unit>) =
        new Task(fun () -> Async.RunSynchronously a)
        // Task.Factory.StartNew(fun () -> Async.RunSynchronously a)
            
    let awaitPlainTask(t: Task) =
        t.ContinueWith (fun _ -> ()) |> Async.AwaitTask
        
    let unit a =
        async {
            return a
        }
        
    let bind f a =
        async {
            let! result = a
            return! f result
        }
        
    let map f a =
        async {
            let! result = a
            return f result
        }
