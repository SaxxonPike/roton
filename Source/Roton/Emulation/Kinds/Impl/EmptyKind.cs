using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x00)]
[Context(Context.Super, 0x00)]
public class EmptyKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = ' ';
        element.Color = 0x70;
        element.IsPushable = true;
        element.IsFloor = true;
        element.Name = "Empty";
    }
}

[Context(Context.Original, 0x03)]
public class OriginalMonitorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x20;
        element.Color = 0x07;
        element.Cycle = 1;
        element.Name = "Monitor";
    }
}

[Context(Context.Super, 0x03)]
public class SuperMonitorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x02;
        element.Color = 0x1F;
        element.Cycle = 1;
        element.IsPushable = true;
        element.Name = "Monitor";
    }
}

[Context(Context.Original, 0x13)]
public class WaterKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB0;
        element.Color = 0xF9;
        element.IsEditorFloor = true;
        element.MenuIndex = 3;
        element.MenuKey = 'W';
        element.Name = "Water";
        element.EditorCategory = "Terrains:";
    }
}

[Context(Context.Super, 0x13)]
public class LavaKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x6F;
        element.Color = 0x4E;
        element.IsEditorFloor = true;
        element.MenuIndex = 3;
        element.MenuKey = 'L';
        element.Name = "Lava";
        element.EditorCategory = "Terrains:";
    }
}

[Context(Context.Original, 0x14)]
[Context(Context.Super, 0x14)]
public class ForestKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB0;
        element.Color = 0x20;
        element.IsFloor = true;
        element.MenuIndex = 3;
        element.MenuKey = 'F';
        element.Name = "Forest";
    }
}

[Context(Context.Original, 0x04)]
public class OriginalPlayerKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x02;
        element.Color = 0x1F;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.IsAlwaysVisible = true;
        element.Cycle = 1;
        element.MenuIndex = 1;
        element.MenuKey = 'Z';
        element.Name = "Player";
        element.EditorCategory = "Items:";
    }
}

[Context(Context.Super, 0x04)]
public class SuperPlayerKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x02;
        element.Color = 0x1F;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.IsAlwaysVisible = true;
        element.Cycle = 1;
        element.MenuIndex = 1;
        element.MenuKey = 'Z';
        element.Name = "Player";
        element.EditorCategory = "Items:";
    }
}

[Context(Context.Original, 0x29)]
[Context(Context.Super, 0x29)]
public class LionKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xEA;
        element.Color = 0x0C;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 2;
        element.MenuIndex = 2;
        element.MenuKey = 'L';
        element.Name = "Lion";
        element.EditorCategory = "Beasts:";
        element.P1EditText = "Intelligence?";
        element.Points = 1;
    }
}

[Context(Context.Original, 0x2A)]
[Context(Context.Super, 0x2A)]
public class TigerKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xE3;
        element.Color = 0x0B;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 2;
        element.MenuIndex = 2;
        element.MenuKey = 'T';
        element.Name = "Tiger";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Firing rate?";
        element.P3EditText = "Firing type?";
        element.Points = 2;
    }
}

[Context(Context.Super, 0x3B)]
public class RotonKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x94;
        element.Color = 0x0D;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 1;
        element.MenuIndex = 4;
        element.MenuKey = 'R';
        element.Name = "Roton";
        element.EditorCategory = "Uglies:";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Switch Rate?";
        element.Points = 2;
    }
}

[Context(Context.Super, 0x3C)]
public class DragonPupKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xED;
        element.Color = 0x04;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 2;
        element.HasDrawCode = true;
        element.MenuIndex = 4;
        element.MenuKey = 'D';
        element.Name = "Dragon Pup";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Switch Rate?";
        element.Points = 1;
    }
}

[Context(Context.Super, 0x3D)]
public class PairerKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xE5;
        element.Color = 0x01;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 2;
        element.MenuIndex = 4;
        element.MenuKey = 'P';
        element.Name = "Pairer";
        element.P1EditText = "Intelligence?";
        element.Points = 2;
    }
}

[Context(Context.Super, 0x3E)]
public class SpiderKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x0F;
        element.Color = 0xFF;
        element.IsDestructible = true;
        element.Cycle = 1;
        element.MenuIndex = 4;
        element.MenuKey = 'S';
        element.Name = "Spider";
        element.P1EditText = "Intelligence?";
        element.Points = 3;
    }
}

[Context(Context.Original, 0x2C)]
[Context(Context.Super, 0x2C)]
public class CentipedeHeadKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xE9;
        element.IsDestructible = true;
        element.Cycle = 2;
        element.MenuIndex = 2;
        element.MenuKey = 'H';
        element.Name = "Head";
        element.EditorCategory = "Centipedes";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Deviance?";
        element.Points = 1;
    }
}

[Context(Context.Original, 0x2D)]
[Context(Context.Super, 0x2D)]
public class CentipedeSegmentKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x4F;
        element.IsDestructible = true;
        element.Cycle = 2;
        element.MenuIndex = 2;
        element.MenuKey = 'S';
        element.Name = "Segment";
        element.Points = 3;
    }
}

[Context(Context.Original, 0x12)]
[Context(Context.Super, 0x45)]
public class BulletKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xF8;
        element.Color = 0x0F;
        element.IsDestructible = true;
        element.Cycle = 1;
        element.Name = "Bullet";
    }
}

[Context(Context.Original, 0x0F)]
[Context(Context.Super, 0x48)]
public class StarKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x53;
        element.Color = 0x0F;
        element.Cycle = 1;
        element.HasDrawCode = true;
        element.Name = "Star";
    }
}

[Context(Context.Original, 0x08)]
[Context(Context.Super, 0x08)]
public class KeyKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x0C;
        element.IsPushable = true;
        element.MenuIndex = 1;
        element.MenuKey = 'K';
        element.Name = "Key";
    }
}

[Context(Context.Original, 0x05)]
[Context(Context.Super, 0x05)]
public class AmmoKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x84;
        element.Color = 0x03;
        element.IsPushable = true;
        element.MenuIndex = 1;
        element.MenuKey = 'A';
        element.Name = "Ammo";
    }
}

[Context(Context.Super, 0x40)]
public class StoneKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 'Z';
        element.Color = 0x0F;
        element.Cycle = 1;
        element.HasDrawCode = true;
        element.MenuIndex = 5;
        element.MenuKey = 'Z';
        element.Name = "Stone";
    }
}

[Context(Context.Original, 0x07)]
[Context(Context.Super, 0x07)]
public class GemKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x04;
        element.IsPushable = true;
        element.IsDestructible = true;
        element.MenuIndex = 1;
        element.MenuKey = 'G';
        element.Name = "Gem";
    }
}

[Context(Context.Original, 0x0B)]
public class OriginalPassageKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xF0;
        element.Cycle = 0;
        element.IsAlwaysVisible = true;
        element.MenuIndex = 1;
        element.MenuKey = 'P';
        element.Name = "Passage";
        element.BoardEditText = "Room thru passage?";
    }
}

[Context(Context.Super, 0x0B)]
public class SuperPassageKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xF0;
        element.Cycle = 0;
        element.IsAlwaysVisible = true;
        element.MenuIndex = 1;
        element.MenuKey = 'P';
        element.Name = "Passage";
        element.BoardEditText = "Room thru passage?";
    }
}

[Context(Context.Original, 0x09)]
[Context(Context.Super, 0x09)]
public class DoorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x0A;
        element.MenuIndex = 1;
        element.MenuKey = 'D';
        element.Name = "Door";
    }
}

[Context(Context.Original, 0x0A)]
[Context(Context.Super, 0x0A)]
public class ScrollKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xE8;
        element.Color = 0x0F;
        element.IsPushable = true;
        element.Cycle = 1;
        element.MenuIndex = 1;
        element.MenuKey = 'S';
        element.Name = "Scroll";
        element.CodeEditText = "Edit text of scroll";
    }
}

[Context(Context.Original, 0x0C)]
[Context(Context.Super, 0x0C)]
public class DuplicatorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xFA;
        element.Color = 0x0F;
        element.Cycle = 2;
        element.HasDrawCode = true;
        element.MenuIndex = 1;
        element.MenuKey = 'U';
        element.Name = "Duplicator";
        element.StepEditText = "Source direction?";
        element.P2EditText = "Duplication rate?;SF";
    }
}

[Context(Context.Original, 0x06)]
public class TorchKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x9D;
        element.Color = 0x06;
        element.IsAlwaysVisible = true;
        element.MenuIndex = 1;
        element.MenuKey = 'T';
        element.Name = "Torch";
    }
}

[Context(Context.Original, 0x27)]
[Context(Context.Super, 0x27)]
public class SpinningGunKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x18;
        element.Cycle = 2;
        element.HasDrawCode = true;
        element.MenuIndex = 2;
        element.MenuKey = 'G';
        element.Name = "Spinning gun";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Firing rate?";
        element.P3EditText = "Firing type?";
    }
}

[Context(Context.Original, 0x23)]
[Context(Context.Super, 0x23)]
public class RuffianKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x05;
        element.Color = 0x0D;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 1;
        element.MenuIndex = 2;
        element.MenuKey = 'R';
        element.Name = "Ruffian";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Resting time?";
        element.Points = 2;
    }
}

[Context(Context.Original, 0x22)]
public class OriginalBearKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x99;
        element.Color = 0x06;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 3;
        element.MenuIndex = 2;
        element.MenuKey = 'B';
        element.Name = "Bear";
        element.EditorCategory = "Creatures:";
        element.P1EditText = "Sensitivity?";
        element.Points = 1;
    }
}

[Context(Context.Super, 0x22)]
public class SuperBearKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xEB;
        element.Color = 0x02;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 3;
        element.MenuIndex = 2;
        element.MenuKey = 'B';
        element.Name = "Bear";
        element.EditorCategory = "Creatures:";
        element.P1EditText = "Sensitivity?";
        element.Points = 1;
    }
}

[Context(Context.Original, 0x25)]
[Context(Context.Super, 0x25)]
public class SlimeKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = '*';
        element.Cycle = 3;
        element.MenuIndex = 2;
        element.MenuKey = 'V';
        element.Name = "Slime";
        element.P2EditText = "Movement speed?;FS";
    }
}

[Context(Context.Original, 0x26)]
public class SharkKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = '^';
        element.Color = 0x07;
        element.Cycle = 3;
        element.MenuIndex = 2;
        element.MenuKey = 'Y';
        element.Name = "Shark";
        element.P1EditText = "Intelligence?";
    }
}

[Context(Context.Original, 0x10)]
[Context(Context.Super, 0x10)]
public class ClockwiseConveyorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = '/';
        element.Cycle = 3;
        element.HasDrawCode = true;
        element.MenuIndex = 1;
        element.MenuKey = '1';
        element.Name = "Clockwise";
        element.EditorCategory = "Conveyors:";
    }
}

[Context(Context.Original, 0x11)]
[Context(Context.Super, 0x11)]
public class CounterClockwiseConveyorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = '\\';
        element.Cycle = 2;
        element.HasDrawCode = true;
        element.MenuIndex = 1;
        element.MenuKey = '2';
        element.Name = "Counter";
    }
}

[Context(Context.Original, 0x15)]
[Context(Context.Super, 0x15)]
public class SolidKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xDB;
        element.MenuIndex = 3;
        element.EditorCategory = "Walls:";
        element.MenuKey = 'S';
        element.Name = "Solid";
    }
}

[Context(Context.Original, 0x16)]
[Context(Context.Super, 0x16)]
public class NormalKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB2;
        element.MenuIndex = 3;
        element.MenuKey = 'N';
        element.Name = "Normal";
    }
}

[Context(Context.Original, 0x2B)]
[Context(Context.Super, 0x47)]
public class VerticalBlinkWallKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xBA;
    }
}

[Context(Context.Original, 0x21)]
[Context(Context.Super, 0x46)]
public class HorizontalBlinkWallKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xCD;
    }
}

[Context(Context.Original, 0x20)]
[Context(Context.Super, 0x20)]
public class RicochetKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = '*';
        element.Color = 0x0A;
        element.MenuIndex = 3;
        element.MenuKey = 'R';
        element.Name = "Ricochet";
    }
}

[Context(Context.Original, 0x17)]
[Context(Context.Super, 0x17)]
public class BreakableKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB1;
        element.MenuIndex = 3;
        element.MenuKey = 'B';
        element.Name = "Breakable";
    }
}

[Context(Context.Original, 0x18)]
[Context(Context.Super, 0x18)]
public class BoulderKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xFE;
        element.IsPushable = true;
        element.MenuIndex = 3;
        element.MenuKey = 'O';
        element.Name = "Boulder";
    }
}

[Context(Context.Original, 0x19)]
[Context(Context.Super, 0x19)]
public class SliderNsKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x12;
        element.MenuIndex = 3;
        element.MenuKey = '1';
        element.Name = "Slider (NS)";
    }
}

[Context(Context.Original, 0x1A)]
[Context(Context.Super, 0x1A)]
public class SliderEwKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x1D;
        element.MenuIndex = 3;
        element.MenuKey = '2';
        element.Name = "Slider (EW)";
    }
}

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
public class TransporterKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xC5;
        element.HasDrawCode = true;
        element.Cycle = 2;
        element.MenuIndex = 3;
        element.MenuKey = 'T';
        element.Name = "Transporter";
        element.StepEditText = "Direction?";
    }
}

[Context(Context.Original, 0x28)]
[Context(Context.Super, 0x28)]
public class PusherKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x10;
        element.HasDrawCode = true;
        element.Cycle = 4;
        element.MenuIndex = 2;
        element.MenuKey = 'P';
        element.Name = "Pusher";
        element.StepEditText = "Push direction?";
    }
}

[Context(Context.Original, 0x0D)]
[Context(Context.Super, 0x0D)]
public class BombKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x0B;
        element.HasDrawCode = true;
        element.IsPushable = true;
        element.Cycle = 6;
        element.MenuIndex = 1;
        element.MenuKey = 'B';
        element.Name = "Bomb";
    }
}

[Context(Context.Original, 0x0E)]
[Context(Context.Super, 0x0E)]
public class EnergizerKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x7F;
        element.Color = 0x05;
        element.MenuIndex = 1;
        element.MenuKey = 'E';
        element.Name = "Energizer";
    }
}

[Context(Context.Original, 0x1D)]
public class OriginalBlinkWallKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xCE;
        element.Cycle = 1;
        element.HasDrawCode = true;
        element.MenuIndex = 3;
        element.MenuKey = 'L';
        element.Name = "Blink wall";
        element.P1EditText = "Starting time";
        element.P2EditText = "Period";
        element.StepEditText = "Wall direction";
    }
}

[Context(Context.Super, 0x1D)]
public class SuperBlinkWallKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xCE;
        element.Cycle = 1;
        element.HasDrawCode = true;
        element.MenuIndex = 3;
        element.MenuKey = 'X';
        element.Name = "Blink wall";
        element.P1EditText = "Starting time";
        element.P2EditText = "Period";
        element.StepEditText = "Wall direction";
    }
}

[Context(Context.Original, 0x1B)]
[Context(Context.Super, 0x1B)]
public class FakeWallKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB2;
        element.MenuIndex = 3;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = 'A';
        element.Name = "Fake";
    }
}

[Context(Context.Super, 0x2F)]
public class FloorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB0;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = 'F';
        element.Name = "Floor";
        element.EditorCategory = "Terrains:";
    }
}

[Context(Context.Super, 0x3F)]
public class WebKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xC5;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.HasDrawCode = true;
        element.MenuKey = 'W';
        element.Name = "Web";
    }
}

[Context(Context.Super, 0x30)]
public class WaterNKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x1E;
        element.Color = 0x19;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = '8';
        element.Name = "Water N";
    }
}

[Context(Context.Super, 0x31)]
public class WaterSKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x1F;
        element.Color = 0x19;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = '2';
        element.Name = "Water S";
    }
}

[Context(Context.Super, 0x32)]
public class WaterWKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x11;
        element.Color = 0x19;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = '4';
        element.Name = "Water W";
    }
}

[Context(Context.Super, 0x33)]
public class WaterEKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x10;
        element.Color = 0x19;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = '6';
        element.Name = "Water E";
    }
}

[Context(Context.Original, 0x1C)]
[Context(Context.Super, 0x1C)]
public class InvisibleKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x20;
        element.MenuIndex = 3;
        element.MenuKey = 'I';
        element.Name = "Invisible";
    }
}

[Context(Context.Original, 0x24)]
[Context(Context.Super, 0x24)]
public class ObjectKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x02;
        element.MenuIndex = 2;
        element.Cycle = 3;
        element.HasDrawCode = true;
        element.MenuKey = 'O';
        element.Name = "Object";
        element.P1EditText = "Character?";
        element.CodeEditText = "Edit Program";
    }
}