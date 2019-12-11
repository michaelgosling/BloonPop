#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Modified By: Michael Gosling <MGosling94@gmail.com>
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace BalloonPop {
  /// <summary>
  /// The main menu screen is the first thing displayed when the game starts up.
  /// </summary>
  public class MainMenuScreen : MenuScreen {
    #region State Data


    /// <summary>
    /// The potential states of the main menu.
    /// </summary>
    enum MainMenuState {
      Empty,
      Title,
      Paused
    }

    bool updateState;

    /// <summary>
    /// The current state of the main menu.
    /// </summary>
    MainMenuState state = MainMenuState.Empty;
    MainMenuState State {
      get { return state; }
      set {
        // exit early from trivial sets
        if (state == value && !updateState) {
          return;
        }
        updateState = false;  // reset the flag, in case it was set
        state = value;
        if (MenuEntries != null) {
          switch (state) {
            case MainMenuState.Title:
              MenuEntries.Clear();
              MenuEntries.Add("Start Game");
              MenuEntries.Add("Exit");
              break;
            case MainMenuState.Paused:
              MenuEntries.Clear();
              MenuEntries.Add("Return To Main Menu");
              MenuEntries.Add("Quit To Desktop");
              break;
          }
        }
      }
    }


    #endregion


    #region Initialization


    /// <summary>
    /// Constructs a new MainMenu object.
    /// </summary>
    public MainMenuScreen() : base() {
      // set the transition times
      TransitionOnTime = TimeSpan.FromSeconds(1.0);
      TransitionOffTime = TimeSpan.FromSeconds(0.0);

      updateState = false;
    }


    #endregion


    #region Updating Methods


    /// <summary>
    /// Updates the screen. This method checks the GameScreen.IsActive
    /// property, so the game will stop updating when the pause menu is active,
    /// or if you tab away to a different application.
    /// </summary>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
        bool coveredByOtherScreen) {

      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }


    /// <summary>
    /// Responds to user menu selections.
    /// </summary>
    protected override void OnSelectEntry(int entryIndex) {
      switch (state) {
        default:
          break;
      }
    }

    /// <summary>
    /// When the user cancels the main menu, ask if they want to exit the sample.
    /// </summary>
    protected override void OnCancel() {
      // const string message = "Exit Net Rumble?";
      // MessageBoxScreen messageBox = new MessageBoxScreen(message);
      // messageBox.Accepted += ExitMessageBoxAccepted;
      // ScreenManager.AddScreen(messageBox);
    }


    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to exit" message box.
    /// </summary>
    void ExitMessageBoxAccepted(object sender, EventArgs e) {
      ScreenManager.Game.Exit();
    }

    #endregion

    /// <summary>
    /// Event handler for when the user selects ok on the network-operation-failed
    /// message box.
    /// </summary>
    void FailedMessageBox(object sender, EventArgs e) { }

  }
}
