using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHamster.String
{

    public class ENKR
    {
        public static Dictionary<char, string> set = new Dictionary<char, string>() {
    {'ㅂ' , "q" },{'ㅈ' , "w" },{'ㄷ' , "e" }, {'ㄱ' , "r" }, {'ㅅ' , "t" },{'ㅛ' , "y" },  {'ㅕ' , "u" },
    {'ㅑ' , "i" }, {'ㅐ' , "o" }, {'ㅔ' , "p" },   {'ㅁ' , "a" },  {'ㄴ' , "s" },   {'ㅇ' , "d" },{'ㄹ' , "f" },   {'ㅎ' , "g" },{'ㅗ' , "h" },  {'ㅓ' , "j" },
    {'ㅏ' , "k" },  {'ㅣ' , "l" },   {'ㅋ' , "z" },  {'ㅌ' , "x" },  {'ㅊ' , "c" }, {'ㅡ' , "m" },{'ㅘ' , "hk" }, {'ㅙ' , "ho" },
    {'ㅚ' , "hl" },{'ㅝ' , "nj" },{'ㅞ' , "np" },  {'ㅢ' , "ml" }, {'ㄳ' , "rt" }, {'ㄵ' , "sw" }, {'ㄶ' , "sg" },   {'ㄺ' , "fr" },  {'ㄻ' , "fa" },{'ㄼ' , "fq" }, {'ㄽ' , "ft" }, {'ㄾ' , "fx" }, {'ㄿ' , "fb" },
    {'ㅃ' , "shift+q" },
    {'ㅉ' , "shift+w" },
    {'ㄸ' , "shift+e" },
    {'ㄲ' , "shift+r" },
    {'ㅆ' , "shift+t" },
        };


        public static List<string> GetKoreanWordToEnglish(string kr)
        {
            List<string> en = new List<string>();
            foreach(var str in kr){
                var chars = KoreanToCharArray(str.ToString());
                set.TryGetValue(chars[0], out var a);
                set.TryGetValue(chars[1], out var b);
                set.TryGetValue(chars[2], out var c);
                if(!string.IsNullOrEmpty((a + b + c)))
                {
                    en.Add(a + b + c);
                    Console.Write(a + b + c);
                }
                
            }
            Console.WriteLine();
            return en;
        }
        public static char[] KoreanToCharArray(string origString)
        {
            if (string.IsNullOrEmpty(origString))
                return null;

            char origChar = origString.ToCharArray(0, 1)[0];
            int unicode = Convert.ToInt32(origChar);
            uint jongIndex = 0;
            uint jungIndex = 0;
            uint choIndex = 0; 
            if (unicode < 44032 || unicode > 55203)
            {
                Console.WriteLine("한글이 아닙니다 => " + origString);
                return new char[] { Char.Parse(origString) , ' ', ' ' };
            }
            else
            { 
                uint uCode = Convert.ToUInt32(origChar - '\xAC00');
                jongIndex = uCode % 28;
                jungIndex = ((uCode - jongIndex) / 28) % 21;
                choIndex = ((uCode - jongIndex) / 28) / 21;
                char[] choChar = new char[] { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
                char[] jungChar = new char[] { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
                char[] jongChar = new char[] { ' ', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
                var str = string.Format("{0}의 초성 : {1}, 중성 : {2}, 종성 : {3}", origString, choChar[choIndex].ToString(), jungChar[jungIndex].ToString(), jongChar[jongIndex].ToString());
                var cho = choChar[choIndex];
                var jung = jungChar[jungIndex];
                var jong = jongChar[jongIndex];
                return new char[] { cho, jung, jong };
            }

        }

    }
}
