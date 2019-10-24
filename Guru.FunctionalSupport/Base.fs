namespace Com.Guru.FunctionalSupport

module Base =
    let rec catch expr =
        match expr with
        | Done value
