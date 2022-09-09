using System;

namespace NetWork
{
    class Const
    {
        public const int BUFFER_SIZE = 1024*2*1024;//2M

        public const byte RECONNECT_TIMES = 3;

        public const int NET_STATE_NORMAL = 0;
        public const int NET_STATE_NOPLUG = 101;        
    }
}
