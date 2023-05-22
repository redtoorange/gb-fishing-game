using System;

namespace Game
{
    [Serializable]
    public enum GamePhase
    {
        SETUP,
        ROTATE_BOAT,
        MOVE_BOAT,
        MOVE_FISH,
        DRIFT,
        CATCH_FISH,
        GAME_OVER,
        GAME_WIN
    }
}