namespace Com.Guru.FunctionalSupport
open Com.Guru.FunctionalSupport
open System

module ConfigF =
    let env key =
        try 
            Environment.GetEnvironmentVariable key |> StringF.asOption
        with
        | _ -> None
            

