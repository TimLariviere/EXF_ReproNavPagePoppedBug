// Copyright 2018 Elmish.XamarinForms contributors. See LICENSE.md for license.
namespace EXF_ReproNavPagePoppedBug

open System.Diagnostics
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms

module App = 
    type Model = 
        {
            Depth: int
            ManualPop: bool
        }


    type Msg = NavigateToChildPage | NavigateBack | NavigationPopped

    let init () = { Depth = 0; ManualPop = false }, Cmd.none

    let update msg model =
        match msg with
        | NavigateToChildPage -> { model with Depth = model.Depth + 1 }, Cmd.none
        | NavigateBack -> { model with Depth = model.Depth - 1; ManualPop = true }, Cmd.none
        | NavigationPopped ->
            match model.ManualPop with
            | true -> { model with ManualPop = false }, Cmd.none
            | false -> { model with Depth = model.Depth - 1 }, Cmd.none

    let view (model: Model) dispatch =
        View.NavigationPage(
            popped=(fun _ -> dispatch NavigationPopped),
            pages=[
                for i in 0 .. 1 .. model.Depth do
                    yield View.ContentPage(
                        title="Depth " + string i,
                        content=View.StackLayout(
                            children=[
                                View.Button(text="Next page", command=(fun () -> dispatch NavigateToChildPage), verticalOptions=LayoutOptions.CenterAndExpand)
                                View.Button(text="Previous page", command=(fun () -> dispatch NavigateBack), verticalOptions=LayoutOptions.CenterAndExpand)
                            ]
                        )
                    )
            ]
        )

type App () as app = 
    inherit Application ()

    let runner = 
        Program.mkProgram App.init App.update App.view
        |> Program.withConsoleTrace
        |> Program.runWithDynamicView app


