using System;

namespace Mizuk.NCrypto.Hashes.Util
{
    //sealed class Block
    //{
    //    public static readonly int Size = 64;
    //    public static implicit operator byte[](Block b)
    //    {
    //        return b._values;
    //    }
    //    public static implicit operator Block(byte[] bs)
    //    {
    //        return new Block(bs);
    //    }

    //    readonly byte[] _values = new byte[Size];

    //    internal Block() { }
    //    internal Block(byte[] values)
    //    {
    //        if (values.Length != Size) throw new ArgumentException("values' length must be 64.");
    //        values.CopyTo(_values, 0);
    //    }

    //    public byte this[int i]
    //    {
    //        get
    //        {
    //            return _values[i];
    //        }
    //        set
    //        {
    //            _values[i] = value;
    //        }
    //    }

    //    public ChunksExact ChunksExact(int n)
    //    {
    //        return _values.ChunksExact(n);
    //    }
    //}
}
