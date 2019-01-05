using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SROMLab1 {
    public class Program {
        static void Main(string[] args) {
        }

        public static string Add(string a, string b) {
            a = CorrLength(a);
            b = CorrLength(b);
            var a_32 = new ulong[a.Length / 8];
            var b_32 = new ulong[b.Length / 8];
            a_32 = ToArr(a, a_32);
            b_32 = ToArr(b, b_32);
            var maxlenght = Math.Max(a_32.Length, b_32.Length);
            var c = new ulong[maxlenght + 1];
            Array.Resize(ref a_32, maxlenght);
            Array.Resize(ref b_32, maxlenght);
            ulong carry = 0;
            for (var i = 0; i < maxlenght; i++) {
                ulong t = a_32[i] + b_32[i] + carry;
                carry = t >> 32;
                c[i] = t & 0xffffffff;
            }
            c[a_32.Length] = carry;
            c = RHZ(c);
            var r = RHZStr(ToStr(c));
            return r;
        }

        public static string Sub(string a, string b) {
            a = CorrLength(a);
            b = CorrLength(b);
            var a_32 = new ulong[a.Length / 8];
            var b_32 = new ulong[b.Length / 8];
            a_32 = ToArr(a, a_32);
            b_32 = ToArr(b, b_32);
            ulong borrow = 0;
            var maxlenght = Math.Max(a_32.Length, b_32.Length);
            Array.Resize(ref a_32, maxlenght);
            Array.Resize(ref b_32, maxlenght);
            var c = new ulong[maxlenght];
            for (var i = 0; i < maxlenght; i++) {
                var temp = a_32[i] - b_32[i] - borrow;
                if (temp > a_32[i]) {
                    c[i] = 0xFFFFFFFF & temp;
                    borrow = 1;
                }
                else {
                    c[i] = (0xFFFFFFFF & temp);
                    borrow = 0;
                }
            }
            c = RHZ(c);
            var r = RHZStr(ToStr(c));
            return r;
        }

        public static string Mul(string a, string b) {
            a = CorrLength(a);
            b = CorrLength(b);
            var a_32 = new ulong[a.Length / 8];
            var b_32 = new ulong[b.Length / 8];
            a_32 = ToArr(a, a_32);
            b_32 = ToArr(b, b_32);
            ulong carry = 0;
            var maxlenght = Math.Max(a_32.Length, b_32.Length);
            Array.Resize(ref a_32, maxlenght);
            Array.Resize(ref b_32, maxlenght);
            var c = new ulong[2 * maxlenght];
            for (int i = 0; i < maxlenght; i++) {
                carry = 0;
                for (int j = 0; j < maxlenght; j++) {
                    ulong temp = c[i + j] + a_32[j] * b_32[i] + carry;
                    c[i + j] = temp & 0xFFFFFFFF;
                    carry = temp >> 32;
                }
                c[i + a_32.Length] = carry;
            }
            RHZ(c);
            var r = RHZStr(ToStr(c));
            return r;

        }

        public static string Div(string a, string b) {
            a = CorrLength(a);
            b = CorrLength(b);
            var a_32 = new ulong[a.Length / 8];
            var b_32 = new ulong[b.Length / 8];
            a_32 = ToArr(a, a_32);
            b_32 = ToArr(b, b_32);
            var k = BitLength(b_32);
            var r = a_32;
            ulong[] q = new ulong[a_32.Length];
            ulong[] Temp = new ulong[a_32.Length];
            ulong[] c = new ulong[a_32.Length];
            Temp[0] = 0x1;

            while (LongCmp(r, b_32) >= 0) {
                var t = BitLength(r);
                c = ShiftBitsToHigh(b_32, t - k);
                if (LongCmp(r, c) == -1) {
                    t = t - 1;
                    c = ShiftBitsToHigh(b_32, t - k);
                }
                r = Subtraction(r, c);
                q = Addition(q, ShiftBitsToHigh(Temp, t - k));
            }
            var res = RHZStr(ToStr(q));
            return res;

        }

        public static string Gorn(string a, string b) {
            a = CorrLength(a);
            b = CorrLength(b);
            var a_32 = new ulong[a.Length / 8];
            var b_32 = new ulong[b.Length / 8];
            a_32 = ToArr(a, a_32);
            b_32 = ToArr(b, b_32);
            ulong[][] D = new ulong[16][];
            int m = b.Length;
            int k = 0;
            ulong[] C = new ulong[1];
            C[0] = 0x1;
            D[0] = new ulong[1] { 1 };
            D[1] = a_32;
            for (int i = 2; i < 16; i++) {
                D[i] = Multiply(D[i - 1], a_32);
                D[i] = RHZ(D[i]);
            }
            for (int j = 0; j < m; j++) {
                var qwer = b[j].ToString();
                int w = Convert.ToInt32(qwer, 16);
                var v = D[w];
                C = Multiply(C, v);
                if (j != m - 1) {
                    for (k = 1; k <= 4; k++) {
                        C = Multiply(C, C);
                        C = RHZ(C);
                    }
                }
            }
            C = RHZ(C);
            var res = RHZStr(ToStr(C));
            return res;
        }


        public static string Power(string a, string b) {
            var Pow_b = b;
            a = CorrLength(a);
            b = CorrLength(b);
            var a_32 = new ulong[a.Length / 8];
            var b_32 = new ulong[b.Length / 8];
            a_32 = ToArr(a, a_32);
            b_32 = ToArr(b, b_32);

            //string Pow_b = Program.UlongToString(b);
            ulong[] C = new ulong[1];
            C[0] = 0x1;
            ulong[][] D = new ulong[16][];
            D[0] = new ulong[1] { 1 };
            D[1] = a_32;
            for (int i = 2; i < 16; i++) {
                D[i] = Multiply(D[i - 1], a_32);
                D[i] = RHZ(D[i]);
            }

            for (int i = 0; i < Pow_b.Length; i++) {
                C = Multiply(C, D[Convert.ToInt32(Pow_b[i].ToString(), 16)]);
                if (i != Pow_b.Length - 1) {
                    for (int k = 1; k <= 4; k++) {
                        C = Multiply(C, C);
                        C = RHZ(C);
                    }
                }
            }
            var res = RHZStr(ToStr(C));
            return res;
        }

        public static string ModPower(string a, string b, string n) {
            var Pow_b = b;
            a = CorrLength(a);
            b = CorrLength(b);
            n = CorrLength(n);
            var a_32 = new ulong[a.Length / 8];
            var b_32 = new ulong[b.Length / 8];
            var n_32 = new ulong[n.Length / 8];
            a_32 = ToArr(a, a_32);
            b_32 = ToArr(b, b_32);
            n_32 = ToArr(n, n_32);
            ulong[] C = new ulong[1];
            C[0] = 0x1;
            ulong[][] D = new ulong[16][];
            D[0] = new ulong[1] { 1 };
            D[1] = a_32;
            for (int i = 2; i < 16; i++) {
                D[i] = Multiply(D[i - 1], a_32);
                D[i] = RHZ(D[i]);
            }

            for (int i = 0; i < Pow_b.Length; i++) {
                C = Multiply(C, D[Convert.ToInt32(Pow_b[i].ToString(), 16)]);
                C = ToArr((Modd(ToStr(C), n)), C);
                if (i != Pow_b.Length - 1) {
                    for (int k = 1; k <= 4; k++) {
                        C = Multiply(C, C);
                        C = ToArr((Modd(ToStr(C), n)), C);
                        C = RHZ(C);
                    }
                }
            }
            var res = RHZStr(ToStr(C));
            return res;
        }



        public static string Modd(string a, string b) {
            a = CorrLength(a);
            b = CorrLength(b);
            var a_32 = new ulong[a.Length / 8];
            var b_32 = new ulong[b.Length / 8];
            a_32 = ToArr(a, a_32);
            b_32 = ToArr(b, b_32);
            var k = BitLength(b_32);
            var r = a_32;
            ulong[] q = new ulong[a_32.Length];
            ulong[] Temp = new ulong[a_32.Length];
            ulong[] c = new ulong[a_32.Length];
            Temp[0] = 0x1;

            while (LongCmp(r, b_32) >= 0) {
                var t = BitLength(r);
                c = ShiftBitsToHigh(b_32, t - k);
                if (LongCmp(r, c) == -1) {
                    t = t - 1;
                    c = ShiftBitsToHigh(b_32, t - k);
                }
                r = Subtraction(r, c);
                q = Addition(q, ShiftBitsToHigh(Temp, t - k));
            }
            var res = RHZStr(ToStr(r));
            return res;

        }

        public static string ModAdd(string a, string b, string n) {
            var c = Add(a, b);
            var r = Modd(c, n);
            return r;
        }

        public static string ModSub(string a, string b, string n) {
            var c = Sub(a, b);
            var r = Modd(c, n);
            return r;
        }

        public static string ModMul(string a, string b, string n) {
            var c = Mul(a, b);
            var r = Modd(c, n);
            return r;
        }

        public static string ModGorn(string a, string b, string n) {
            var c = Gorn(a, b);
            var r = Baret(c, n);
            return r;
        }

        public static string NSD(string a, string b) {
            a = CorrLength(a);
            b = CorrLength(b);
            var a_32 = new ulong[a.Length / 8];
            var b_32 = new ulong[b.Length / 8];
            a_32 = ToArr(a, a_32);
            b_32 = ToArr(b, b_32);
            string twostr = "2";
            twostr = CorrLength(twostr);
            var two = new ulong[twostr.Length / 8];
            two = ToArr(twostr, two);
            var maxlenght = Math.Max(a_32.Length, b_32.Length);
            Array.Resize(ref a_32, maxlenght);
            Array.Resize(ref b_32, maxlenght);
            var r = new ulong[maxlenght];
            var d = new ulong[Math.Min(a_32.Length, b_32.Length)];
            d[0] = 0x1;
            var aa = a_32;
            var bb = b_32;
            while (((aa[0] & 1) == 0) && ((bb[0] & 1) == 0)) {
                aa = Division(aa, two);
                bb = Division(bb, two);
                d = Multiply(d, two);
            }
            while ((aa[0] & 1) == 0) {
                aa = Division(aa, two);
            }
            while (bb[0] != 0) {
                while ((bb[0] & 1) == 0) {
                    bb = Division(bb, two);
                }
                var c = LongCmp(aa, bb);
                ulong[] Min, Max;
                if (c >= 0) {
                    Min = bb;
                    Max = aa;
                }
                else {
                    Min = aa;
                    Max = bb;
                }
                aa = Min;
                bb = (Subtraction(Max, Min));
            }
            d = Multiply(d, aa);

            var res = RHZStr(ToStr(d));
            return res;
        }

        public static string NSK(string b, int bits) {
            b = CorrLength(b);
            var a = new ulong[b.Length / 8];
            a = ToArr(b, a); 

            int t = bits / 32;
            int nbits = bits - t * 32;
            ulong[] c = new ulong[a.Length - t];
            ulong ai, ai_1 = 0;
            for (int i = t; i < a.Length - 1; i++) {
                ai = a[i];
                ai_1 = a[i + 1];
                ai = ai >> nbits;
                ai_1 = ai_1 << (64 - nbits);
                ai_1 = ai_1 >> (64 - nbits);
                c[i - t] = ai | (ai_1 << 32 - nbits);
            }
            c[a.Length - t - 1] = a[a.Length - 1] >> nbits;
            var r = RHZStr(ToStr(c));
            return r;
        }

        public static string Baret(string a, string b) {
            a = CorrLength(a);
            b = CorrLength(b);
            var x = new ulong[a.Length / 8];
            var n = new ulong[b.Length / 8];
            x = ToArr(a, x);
            n = ToArr(b, n);
            ulong[] mu = new ulong[1];
            var s = 2 * BitLength(n);
            var z = new ulong[] { 0x01 };
            var up = ShiftBitsToHigh(z, s);
            mu = Division(up, n);
            var k = BitLength(n);
            var q = NSKK(x, k - 1);
            q = Multiply(q, mu);
            q = NSKK(q, k + 1);
            var r = Subtraction(x, Multiply(q, n));
            if (LongCmp(r, n) >= 0) {
                r = Subtraction(r, n);
            }
            var res = RHZStr(ToStr(r));
            return res;
        }


        public static string RHZStr(string a) {
            var r = a.TrimStart('0');
            //r = 0 + r;
            Console.WriteLine(r);
            return r;
        }

        public static string CorrLength(string a) {
            string z = "0";
            while (a.Length % 8 != 0) {
                a = z + a;
            }
            return a;
        }

        public static ulong[] RHZ(ulong[] a_32) {
            int l = a_32.Length;
            int i = l - 1;
            while (a_32[i] == 0) { i--; }
            var r = new ulong[i + 1];
            Array.Copy(a_32, r, i + 1);
            return r;
        }

        public static ulong[] ToArr(string a, ulong[] a_32) {
            a = CorrLength(a);
            var p_32 = new ulong[a.Length / 8];
            for (int i = 0; i < a.Length; i += 8) {
                p_32[i / 8] = Convert.ToUInt64(a.Substring(i, 8), 16);

            }
            Array.Reverse(a_32);
            Array.Reverse(p_32);

            return p_32;
        }

        public static string ToStr(ulong[] a) {
            string st = string.Concat(a.Select(chunk => chunk.ToString("X").PadLeft(sizeof(ulong), '0')).Reverse()).TrimStart('0');
            return st;
        }

        public static ulong[] Addition(ulong[] a_32, ulong[] b_32) {
            var maxlenght = Math.Max(a_32.Length, b_32.Length);
            var c = new ulong[maxlenght];
            Array.Resize(ref a_32, maxlenght);
            Array.Resize(ref b_32, maxlenght);
            ulong carry = 0;
            for (var i = 0; i < maxlenght; i++) {
                ulong t = a_32[i] + b_32[i] + carry;
                carry = t >> 32;
                c[i] = t & 0xffffffff;
            }
            c = RHZ(c);
            return c;
        }

        public static ulong[] Subtraction(ulong[] a_32, ulong[] b_32) {
            ulong borrow = 0;
            var maxlenght = Math.Max(a_32.Length, b_32.Length);
            Array.Resize(ref a_32, maxlenght);
            Array.Resize(ref b_32, maxlenght);
            var c = new ulong[maxlenght];
            for (var i = 0; i < maxlenght; i++) {
                var temp = a_32[i] - b_32[i] - borrow;
                if (temp > a_32[i]) {
                    c[i] = 0xFFFFFFFF & temp;
                    borrow = 1;
                }
                else {
                    c[i] = (0xFFFFFFFF & temp);
                    borrow = 0;
                }
            }

            return c;
        }

        public static ulong[] Multiply(ulong[] a_32, ulong[] b_32) {
            ulong carry = 0;
            var maxlenght = Math.Max(a_32.Length, b_32.Length);
            Array.Resize(ref a_32, maxlenght);
            Array.Resize(ref b_32, maxlenght);
            var c = new ulong[2 * maxlenght];
            for (int i = 0; i < maxlenght; i++) {
                carry = 0;
                for (int j = 0; j < maxlenght; j++) {
                    ulong temp = c[i + j] + a_32[j] * b_32[i] + carry;
                    c[i + j] = temp & 0xFFFFFFFF;
                    carry = temp >> 32;
                }
                c[i + a_32.Length] = carry;
            }
            RHZ(c);
            return c;
        }

        public static ulong[] Division(ulong[] a_32, ulong[] b_32) {
            var k = BitLength(b_32);
            var r = a_32;
            ulong[] q = new ulong[a_32.Length];
            ulong[] Temp = new ulong[a_32.Length];
            ulong[] c = new ulong[a_32.Length];
            Temp[0] = 0x1;

            while (LongCmp(r, b_32) >= 0) {
                var t = BitLength(r);
                c = ShiftBitsToHigh(b_32, t - k);
                if (LongCmp(r, c) == -1) {
                    t = t - 1;
                    c = ShiftBitsToHigh(b_32, t - k);
                }
                r = Subtraction(r, c);
                q = Addition(q, ShiftBitsToHigh(Temp, t - k));
            }
            return q;
        }

        public static ulong[] NSKK(ulong[] a, int bits) {
            /*b = CorrLength(b);
            var a = new ulong[b.Length / 8];
            a = ToArr(b, a);*/

            int t = bits / 32;
            int nbits = bits - t * 32;
            ulong[] c = new ulong[a.Length - t];
            ulong ai, ai_1 = 0;
            for (int i = t; i < a.Length - 1; i++) {
                ai = a[i];
                ai_1 = a[i + 1];
                ai = ai >> nbits;
                ai_1 = ai_1 << (64 - nbits);
                ai_1 = ai_1 >> (64 - nbits);
                c[i - t] = ai | (ai_1 << 32 - nbits);
            }
            c[a.Length - t - 1] = a[a.Length - 1] >> nbits;
            return c;
        }

        public static ulong[] ShiftBitsToHigh(ulong[] a, int b) {
            int t = b / 32;
            int s = b - t * 32;
            ulong n, carry = 0;
            ulong[] C = new ulong[a.Length + t + 1];
            for (int i = 0; i < a.Length; i++) {
                n = a[i];
                n = n << s;
                C[i + t] = (n & 0xFFFFFFFF) + carry;
                carry = (n & 0xFFFFFFFF00000000) >> 32;
            }
            C[C.Length - 1] = carry;
            return C;
        }

        public static int BitLength(ulong[] a_32) {
            int bits = 0;
            int i = a_32.Length - 1;
            while (a_32[i] == 0) {
                if (i < 0)
                    return 0;
                i--;
            }
            var n = a_32[i];
            while (n > 0) {
                bits++;
                n = n >> 1;
            }
            bits = bits + 32 * i;
            return bits;
        }

        static int LongCmp(ulong[] a_32, ulong[] b_32) {
            var maxlenght = Math.Max(a_32.Length, b_32.Length);
            Array.Resize(ref a_32, maxlenght);
            Array.Resize(ref b_32, maxlenght);
            for (int i = a_32.Length - 1; i > -1; i--) {
                if (a_32[i] < b_32[i]) { return -1; }
                if (a_32[i] > b_32[i]) { return 1; }
            }
            return 0;
        }
    }
}