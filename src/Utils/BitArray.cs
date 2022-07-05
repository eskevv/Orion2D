using System.Linq;

namespace Orion2D;
public class BitArray {

   private bool[] _set;

   public BitArray(byte size)
   {
      _set = new bool[size];
   }

   // __Definitions__

   public void SetBits(params ushort[] bitNumbers)
   {
      for (int x = 0; x < bitNumbers.Length; x++)
      {
         ushort bit_place = bitNumbers[x];
         _set[bit_place] = true;
      }
   }

   public void ClearBits(params ushort[] bitNumbers)
   {
      for (int x = 0; x < bitNumbers.Length; x++)
      {
         ushort bit_place = bitNumbers[x];
         _set[bit_place] = false;
      }
   }

   public void Reset()
   {
      for (int x = 0; x < _set.Length; x++)
      {
         _set[x] = false;
      }
   }

   public void SetAll()
   {
      for (int x = 0; x < _set.Length; x++)
      {
         _set[x] = true;
      }
   }

   public bool Includes(BitArray b)
   {
      for (int x = 0; x < b._set.Length; x++)
      {
         if (b._set[x] && !_set[x]) return false;
      }

      return true;
   }

   public override string ToString() => string.Join("", _set.ToList().Select(x => x ? 1 : 0).Reverse());

}