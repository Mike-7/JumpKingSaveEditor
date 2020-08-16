using JumpKing.MiscEntities.Merchant;
using JumpKing.MiscEntities.OldMan;
using JumpKing.MiscEntities.Raven;
using JumpKing.MiscEntities.WorldItems;
using JumpKing.MiscSystems.LocationText;
using JumpKing.MiscSystems.ScreenEvents;
using JumpKing.SaveThread;
using JumpKing.SaveThread.SaveComponents;
using JumpKing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKing.MiscSystems.Achievements
{
    [Serializable]
    public struct PlayerStats
    {
        public float time;
        public int jumps;
        public int falls;
        public int attempts;
        public int session;
        public int times_won;
        public int _ticks;
    }
}

namespace JumpKing.MiscEntities.WorldItems
{
    public enum Items
    {
        Crown,
        Shoes,
        GoldRing,
        CrownNBP,
        Ruby,
        SnakeRing,
        GiantBoots,
        Silver,
        Cap,
        GnomeHat,
        Tunic,
        YellowShoes,
        CrownOwl,
        CapeOwl,
        BabeGhostItem,
        GhostFragment,
        Shroom,
        BugNote,
        NULL
    }

    [Serializable]
    public struct WorldItemState
    {
        public Items item;
        public int screen;
        public int _x;
        public int _y;
    }
}

namespace JumpKing.MiscEntities.WorldItems.Inventory
{
    [Serializable]
    public struct Inventory
    {
        public List<InventoryItem> items;
    }

    [Serializable]
    public struct InventoryItem
    {
        public Items item;
        public int count;
    }
}

namespace JumpKing.MiscEntities.Merchant
{
    [Serializable]
    public struct MerchantState
    {
        public bool has_introduced;
        public bool sold_shoes;
        public int required_gold;
    }
}

namespace JumpKing.MiscEntities.OldMan
{
    [Serializable]
    public struct OldManQuote
    {
        public string[] lines;
    }

    [Serializable]
    public struct OldManFlagQuote
    {
        public StoryEventFlags flag;
        public List<OldManQuote> quotes;
    }

    [Serializable]
    public struct OldManState
    {
        public string name;
        public int best_screen;
        public List<OldManQuote> quote_pool;
        public bool say_intro;
        public OldManQuote intro_quote;
        public int times_talked;
        public List<OldManFlagQuote> flag_quotes;
    }
}

namespace JumpKing.MiscEntities.Raven
{
    [Serializable]
    public struct RavenState
    {
        public int home_screen;
        public bool is_done;
        public bool picked_up_gold;
    }
}

namespace JumpKing.MiscSystems.LocationText
{
    [Serializable]
    public struct LocationState
    {
        public int best_location;
    }
}

namespace JumpKing.SaveThread.SaveComponents
{
    [Serializable]
    public struct WorldItemsSave
    {
        public List<WorldItemState> items;
    }

    [Serializable]
    public struct MerchantSaves
    {
        public List<MerchantSaves.MerchantPair> merchant_pairs;

        [Serializable]
        public struct MerchantPair
        {
            public string name;
            public MerchantState state;
        }
    }

    [Serializable]
    public struct RavenSaves
    {
        public List<RavenSaves.RavenPair> _data;

        [Serializable]
        public struct RavenPair
        {
            public string name;
            public RavenState state;
        }
    }

    [Serializable]
    public struct FullRunSave
    {
        public bool wear_giant_boots;
        public bool wear_snake_ring;
    }

    [Serializable]
    public struct SaveCompCushion<T> where T : struct
    {
        public bool initialized;
        public T m_save;
    }
}

namespace JumpKing.MiscSystems.ScreenEvents
{
    [Serializable]
    public struct ScreenEventSave
    {
        public List<int> registered_screens;
    }
}

namespace JumpKing.SaveThread
{
    [Serializable]
    public enum StoryEventFlags
    {
        CompletedNormalGame,
        CompletedNBP,
        StartedNBP,
        CompletedGhost,
        StartedGhost
    }

    [Serializable]
    public struct SaveState
    {
        public struct Vector2
        {
            public float X;
            public float Y;

            public Vector2(float X, float Y)
            {
                this.X = X;
                this.Y = Y;
            }
        }

        public Vector2 position
        {
            get
            {
                return new Vector2(this.pos_x, this.pos_y);
            }
            set
            {
                this.pos_x = value.X;
                this.pos_y = value.Y;
            }
        }

        public Vector2 velocity
        {
            get
            {
                return new Vector2(this.vel_x, this.vel_y);
            }
            set
            {
                this.vel_x = value.X;
                this.vel_y = value.Y;
            }
        }

        private float pos_x;
        private float pos_y;
        private float vel_x;
        private float vel_y;
        public SaveState.PlayerState state;
        public int time_stamp;
        public bool is_on_ground;
        public int direction;

        public enum PlayerState
        {
            Normal,
            Knocked,
            Splat
        }
    }

    [Serializable]
    public struct CombinedSaveFile
    {
        public MerchantState merchant;
        public RavenState raven;
        public List<CombinedSaveFile.OldManPair> old_men;
        public LocationState location_state;
        public SaveState player_position;
        public SaveCompCushion<WorldItemsSave> world_items;
        public SaveCompCushion<MerchantSaves> merchant_states;
        public SaveCompCushion<ScreenEventSave> screen_events;
        public SaveCompCushion<RavenSaves> raven_states;
        public SaveCompCushion<FullRunSave> full_run;

        [Serializable]
        public struct OldManPair
        {
            public string name;
            public OldManState state;
        }
    }
}
