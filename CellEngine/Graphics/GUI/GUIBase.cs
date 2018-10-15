using System;
using CellEngine.Utils;

namespace CellEngine.Graphics.GUI
{
    public enum Pivot //TODO move?
    {
        Center, BottomLeft, Left, TopLeft, Top, TopRight, Right, BottomRight, Bottom
    }

    public abstract class GUIBase
    {
        public Vector2 Position;
        public Pivot Pivot = Pivot.BottomLeft; //TODO set pivot by coordinates?
        public Pivot? ParentPivot = null;
        public float SizeX;
        public float SizeY;
        public Color Color;
        public bool Enabled = true;
        public GUIBase Parent;

        internal bool updated; //TODO hack
        protected Vector2 globalPos;

        public Window Window { get; private set; }

        private bool clickable;
        public bool Clickable
        {
            get { return clickable; }
            set
            {
                clickable = value;
                if (value)
                    Input.SubscribeMouseClick(OnClick, _z);
                else
                    Input.UnsubscribeMouseClick(OnClick);
            }
        }

        private int _z = 1;
        public int z
        {
            get { return _z; }
            set
            {
                _z = value;
                if (clickable)
                {
                    Input.UnsubscribeMouseClick(OnClick);
                    Input.SubscribeMouseClick(OnClick, _z);
                }
                Window.SortGUI();
            }
        }

        public event Action<int> Clicked;

        public GUIBase(Window window, Color color, Vector2 pos, float sizeX, float sizeY, int z = 1)
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            Color = color;
            Position = pos;

            Window = window;
            window.AddGUIElement(this);

            this.z = z;
            Clickable = true;
        }

        internal void Render()
        {
            UpdatePosition(); //TODO maybe static rendering? do we really need to do this every frame?
            Draw();
        }

        protected abstract void Draw();
        protected abstract void UpdateModel();

        private void UpdatePosition()
        {
            if (updated) return;
            Parent?.UpdatePosition();

            Vector2 offset = Parent?.GetPivotOffset(ParentPivot ?? Parent.Pivot) ?? Vector2.zero;
            Vector2 parentPos = Parent == null ? Vector2.zero : new Vector2(Parent.globalPos.x, Parent.globalPos.y) + offset;
            globalPos = parentPos + Position - GetPivotOffset(Pivot);

            UpdateModel();
            updated = true;
        }

        public Vector2 GetPivotOffset(Pivot pivot)
        {
            switch (pivot)
            {
                case Pivot.Center:
                    return new Vector2(SizeX / 2, SizeY / 2);
                case Pivot.BottomLeft:
                    return Vector2.zero;
                case Pivot.Left:
                    return new Vector2(0, SizeY / 2);
                case Pivot.TopLeft:
                    return new Vector2(0, SizeY);
                case Pivot.Top:
                    return new Vector2(SizeX / 2, SizeY);
                case Pivot.TopRight:
                    return new Vector2(SizeX, SizeY);
                case Pivot.Right:
                    return new Vector2(SizeX, SizeY / 2);
                case Pivot.BottomRight:
                    return new Vector2(SizeX, 0);
                case Pivot.Bottom:
                    return new Vector2(SizeX / 2, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(pivot), pivot, null);
            }
        }

        //Fires on mouse click
        private bool OnClick(Vector2 pos, int button)
        {
            if (Clickable && Enabled && IsClicked(pos, button))
            {
                Clicked?.Invoke(button);
                return true;
            }
            return false;
        }

        //Checks if element was clicked by it's bounds
        protected bool IsClicked(Vector2 pos, int button) //TODO maybe override?
        {
            return pos.x <= globalPos.x + SizeX && pos.y <= globalPos.y + SizeY && pos.x >= globalPos.x && pos.y >= globalPos.y;
        }
    }
}