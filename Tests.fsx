#load "Code.fsx"
open Code

let shouldEqual actual expected = if expected <> actual then failwithf "Expected %A but got %A" expected actual else printfn "Passed"

module ``Hof Tests`` =
    let ``Correctly validates success``() =
        let mockLoad _ = { Name = "Fred"; Age = 31 }
        let mockSave _ = Ok()
        let result = Hof.orchestrator mockLoad mockSave 123
        Ok() |> shouldEqual result

    let ``Correctly validates failure``() =
        let mockLoad _ = { Name = "Fred"; Age = 17 }
        let mockSave _ = Ok()
        let result = Hof.orchestrator mockLoad mockSave 123
        Error "Too young!" |> shouldEqual result

    // TODO: Did orchestrator call mock load with correct arguments?
    // TODO: Did orchestrator call mock save with correct arguments?

module ``Bootstrap Tests`` =
    let ``Can test orchestration without validation``() =
        let mockLoad _ = { Name = "Fred"; Age = 17 }
        let mockSave _ = Ok()
        let result = Bootstrap.orchestrator mockLoad mockSave 123
        Ok() |> shouldEqual result

    // TODO: Did orchestrator call mock load with correct arguments?
    // TODO: Did orchestrator call mock save with correct arguments?

    let ``Validation still coupled to data access``() =
        let mockSave _ = Ok()
        let result = Bootstrap.validateThenSave mockSave { Name = "Fred"; Age = 17 }
        Error "Too young!" |> shouldEqual result

module ``Compose Tests`` =
    let ``Correctly validates success``() =
        let p = { Name = "Fred"; Age = 31 }
        let result = Compose.validate p
        Ok p |> shouldEqual result

    let ``Correctly validates failure``() =
        let p = { Name = "Fred"; Age = 17 }
        let result = Compose.validate p
        Error "Too young!" |> shouldEqual result

    // TODO: Did orchestrator call mock load with correct arguments?
    // TODO: Did orchestrator call mock save with correct arguments?

// Composition-to-the-Max tests are as per Compose tests, but no ability / need to test
// orchestration function - it doesn't exist.