using System.Collections.Generic;
using AgateLib.Mathematics.Geometry;
using AgateLib.InputLib;
using ERY.Xle.Services.Commands;
using ERY.Xle.Services.Rendering.Maps;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.Maps
{
    public interface IMapExtender
    {
        XleMap TheMap { get; }
        int WaitTimeAfterStep { get; }
        XleMapRenderer MapRenderer { get; }
        IReadOnlyList<EventExtender> Events { get; }

        int MapID { get; }
        bool IsAngry { get; set; }

        IEnumerable<EventExtender> EventsAt(int v);
        void SetCommands(ICommandList commands);
        void PlayerCursorMovement(Direction dir);
        void OnUpdate(double v);
        void OnLoad();
        void OnAfterEntry();
        void ModifyEntryPoint(MapEntryParams entryParams);
        void LeaveMap();
        void CheckSounds();
        void AfterExecuteCommand(KeyCode cmd);
        bool CanPlayerStepInto(Point corridorPt);
        bool CanPlayerStepIntoImpl(int v, int targetY);
    }
}