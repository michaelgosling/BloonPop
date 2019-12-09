using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloonPop {
    static class Constants {
        public static readonly int WINDOW_WIDTH = 900;
        public static readonly int WINDOW_HEIGHT = 800;
        public static readonly int BALLOON_SPEED = 7;
        public static readonly float PLAYER_ROTATE_AMT = 0.04f;

        public static readonly int PLAY_AREA_WIDTH = 512;
        public static readonly Rectangle PLAY_AREA = new Rectangle(WINDOW_WIDTH / 2 - PLAY_AREA_WIDTH / 2,
            0, PLAY_AREA_WIDTH, WINDOW_HEIGHT);

        /** Levels */
        public static readonly BalloonColor[] LEVEL1_BALLOONS = {
            BalloonColor.Color1,
            BalloonColor.Color1,
            BalloonColor.Color2,
            BalloonColor.Color2,
            BalloonColor.Color3,
            BalloonColor.Color3
        };
        public static readonly BalloonColor[] LEVEL2_BALLOONS = {
            BalloonColor.Color1,
            BalloonColor.Color3,
            BalloonColor.Color5,
            BalloonColor.Color6,
            BalloonColor.Color3,
            BalloonColor.Color5,
            BalloonColor.Color1,
            BalloonColor.Color2,
            BalloonColor.Color4,
            BalloonColor.Color1,
            BalloonColor.Color5
        };
        public static readonly BalloonColor[] LEVEL3_BALLOONS = {
            BalloonColor.Color2,
            BalloonColor.Color3,
            BalloonColor.Color1,
            BalloonColor.Color5,
            BalloonColor.Color1,
            BalloonColor.Color2,
            BalloonColor.Color1,
            BalloonColor.Color3,
            BalloonColor.Color1,
            BalloonColor.Color6,
            BalloonColor.Color3,
            BalloonColor.Color6,
            BalloonColor.Color1,
            BalloonColor.Color5,
            BalloonColor.Color3,
            BalloonColor.Color5,
            BalloonColor.Color3,
            BalloonColor.Color2,
            BalloonColor.Color1,
            BalloonColor.Color5,
            BalloonColor.Color2,
            BalloonColor.Color6,
            BalloonColor.Color1,
            BalloonColor.Color3,
            BalloonColor.Color6,
            BalloonColor.Color3,
            BalloonColor.Color1,
            BalloonColor.Color6,
            BalloonColor.Color5,
            BalloonColor.Color2
        };
        /** End Levels */

    }
}