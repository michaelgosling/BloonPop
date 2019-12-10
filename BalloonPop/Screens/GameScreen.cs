#region File Description
/*
 GameScreen.cs

 Modified By: Michael Gosling <MGosling94@gmail.com>

 Microsoft XNA Community Game Platform
 Copyright (C) Microsoft Corporation. All rights reserved.
 */
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace BalloonPop {

  /// <summary>
  /// Enum describes the transition state.
  /// </summary>
  public enum ScreenState {
    TransitionOn,
    Active,
    TransitionOff,
    Hidden
  }

  /// <summary>
  /// A screen is a single layer that has update and draw logic, and which
  /// can be combined with other layers to build up a complex menu system.
  /// For instance the main menu, the options menu, the "are you sure you
  /// want to quit" message box, and the main game itself are all implemented
  /// as screens.
  /// </summary>
  public abstract class GameScreen {
    #region Properties

    /// <summary>
    /// Indicates if the screen is only a small popup, in which case screens underneath
    /// it do not need to bother transitioning off.
    /// </summary>
    /// <value></value>
    public bool IsPopup { get; protected set; } = false;
    public TimeSpan TransitionOnTime { get; protected set; } = TimeSpan.Zero;
    public TimeSpan TransitionOffTime { get; protected set; } = TimeSpan.Zero;
    public float TransitionPosition { get; protected set; } = 1;
    public byte TransitionAlpha { get { return (byte)(255 - TransitionPosition * 255); } }
    public ScreenState ScreenState { get; protected set; } = ScreenState.TransitionOn;
    public bool IsExiting { get; protected set; } = false;
    public bool IsActive { get { return !otherScreenHasFocus && (ScreenState == ScreenState.TransitionOn || ScreenState == ScreenState.Active); } }
    bool otherScreenHasFocus;
    public ScreenManager ScreenManager { get; protected set; }
    #endregion

    #region Initialization
    public virtual void LoadContent() { }
    public virtual void UnloadContent() { }
    #endregion

    #region Update & Draw

    public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {

    }

    /// <summary>
    /// Helper for updating the screen transition positiong
    /// </summary>
    /// <param name="gameTime">Game Time since last update</param>
    /// <param name="time"></param>
    /// <param name="direction">Direction of transition</param>
    /// <returns>True if still transitioning, false if finished</returns>
    bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction) {
      // Determine how much transition should move.
      float transDelta = time == TimeSpan.Zero ? 1 : (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

      // Update transition position
      TransitionPosition += transDelta * direction;

      // is transition over? 
      if (TransitionPosition <= 0 || TransitionPosition >= 1) {
        TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
        return false;
      }

      // otherwise we're still transitioning
      return true;
    }

    public virtual void HandleInput(InputState input) { }

    public virtual void UpdatePresence() { }

    public abstract void Draw(GameTime gameTime);

    #endregion

    #region Public Methods

    /// <summary>
    /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which instantly kills the screen, this method
    /// respects the transition timings and will give the screen a change to gradually transition off. 
    /// </summary>
    public virtual void ExitScreen() {
      if (TransitionOffTime == TimeSpan.Zero) ScreenManager.RemoveScreen(this);
      IsExiting = true;
    }

    #endregion

  }
}