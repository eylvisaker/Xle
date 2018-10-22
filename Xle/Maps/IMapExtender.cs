using Xle.Services.Commands;
using Xle.Services.Rendering.Maps;
using Xle.XleEventTypes.Extenders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Xle.Maps
{
    public interface IMapExtender
    {
        XleMap TheMap { get; set; }
        int WaitTimeAfterStep { get; }
        XleMapRenderer MapRenderer { get; }
        IReadOnlyList<EventExtender> Events { get; }

        int MapID { get; }
        bool IsAngry { get; set; }

        IEnumerable<EventExtender> EventsAt(int v);
        void SetCommands(ICommandList commands);
        void PlayerCursorMovement(Direction dir);
        void OnUpdate(GameTime time);
        void OnLoad();
        void OnAfterEntry();
        void ModifyEntryPoint(MapEntryParams entryParams);
        void LeaveMap();
        void CheckSounds(GameTime time);
        void AfterExecuteCommand(Keys cmd);
        bool CanPlayerStepInto(Point corridorPt);
        bool CanPlayerStepIntoImpl(int v, int targetY);
    }
}