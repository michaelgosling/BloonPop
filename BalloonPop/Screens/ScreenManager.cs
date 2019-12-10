#region File Description
/*
 ScreenManager.cs

 Modified By: Michael Gosling <MGosling94@gmail.com>

 Microsoft XNA Community Game Platform
 Copyright (C) Microsoft Corporation. All rights reserved.
 */
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace BalloonPop {
  public class ScreenManager : DrawableGameComponent {
    #region Fields

    List<GameScreen> screens = new List<GameScreen>();

    #endregion
  }
}