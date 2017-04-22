using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab_1
{
    public class ConvolutionMatrix
    {
        public int[] matrix3x3;
        public int[] matrix5x5;
        public int[] matrix7x7;

        public ConvolutionMatrix(int[] matrix)
        {
            matrix3x3 = matrix;
            ExpandTo5x5();
            ExpandTo7x7();
        }

        private void ExpandTo5x5()
        {
            matrix5x5 = new int[25];

            matrix5x5[0] = matrix5x5[1] = matrix5x5[6] = matrix3x3[0];
            matrix5x5[2] = matrix5x5[3] = matrix5x5[7] = matrix3x3[1];
            matrix5x5[4] = matrix5x5[8] = matrix5x5[9] = matrix3x3[2];
            matrix5x5[5] = matrix5x5[10] = matrix5x5[11] = matrix3x3[3];
            matrix5x5[12] = matrix3x3[4];
            matrix5x5[13] = matrix5x5[14] = matrix5x5[19] = matrix3x3[5];
            matrix5x5[15] = matrix5x5[16] = matrix5x5[20] = matrix3x3[6];
            matrix5x5[17] = matrix5x5[21] = matrix5x5[22] = matrix3x3[7];
            matrix5x5[18] = matrix5x5[23] = matrix5x5[24] = matrix3x3[8];
        }

        private void ExpandTo7x7()
        {
            matrix7x7 = new int[49];

            matrix7x7[0] = matrix7x7[1] = matrix7x7[8] = matrix7x7[9] = matrix7x7[16] = matrix3x3[0];
            matrix7x7[3] = matrix7x7[4] = matrix7x7[10] = matrix7x7[11] = matrix7x7[17] = matrix3x3[1];
            matrix7x7[6] = matrix7x7[12] = matrix7x7[13] = matrix7x7[18] = matrix7x7[19] = matrix3x3[2];
            matrix7x7[14] = matrix7x7[15] = matrix7x7[21] = matrix7x7[22] = matrix7x7[23] = matrix3x3[3];
            matrix7x7[24] = matrix3x3[4];
            matrix7x7[25] = matrix7x7[26] = matrix7x7[27] = matrix7x7[33] = matrix7x7[34] = matrix3x3[5];
            matrix7x7[29] = matrix7x7[30] = matrix7x7[35] = matrix7x7[36] = matrix7x7[42] = matrix3x3[6];
            matrix7x7[31] = matrix7x7[37] = matrix7x7[38] = matrix7x7[44] = matrix7x7[45] = matrix3x3[7];
            matrix7x7[32] = matrix7x7[39] = matrix7x7[40] = matrix7x7[47] = matrix7x7[48] = matrix3x3[8];
            matrix7x7[2] = matrix7x7[5] = matrix7x7[7] = matrix7x7[20] = matrix7x7[28] = matrix7x7[41] = matrix7x7[43] = matrix7x7[46] = 0;
        }
    }
}
