namespace Epic_2._1___Hash_function_for_passwords;
public class SHA256
{
    // Initialization vectors
    private static readonly uint[] H = {
    0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
    0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
};

    // Constants for each round
    private static readonly uint[] K = {
    0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5,
    0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
    0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
    0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
    0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc,
    0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
    0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
    0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
    0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
    0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
    0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3,
    0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
    0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5,
    0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
    0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
    0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
};

    public static byte[] ComputeHash(byte[] data)
    {
        // Data preparation
        uint[] h = (uint[])H.Clone();
        byte[] paddedData = PadData(data);

        // Process data in 512-bit blocks
        for (int i = 0; i < paddedData.Length; i += 64)
        {
            uint[] w = new uint[64];

            // Break the block into 16 words (32 bits each)
            for (int j = 0; j < 16; ++j)
                w[j] = BitConverter.ToUInt32(paddedData, i + j * 4);

            // Extend the 16 words to 64
            for (int j = 16; j < 64; ++j)
            {
                uint s0 = RightRotate(w[j - 15], 7) ^ RightRotate(w[j - 15], 18) ^ (w[j - 15] >> 3);
                uint s1 = RightRotate(w[j - 2], 17) ^ RightRotate(w[j - 2], 19) ^ (w[j - 2] >> 10);
                w[j] = w[j - 16] + s0 + w[j - 7] + s1;
            }

            // Initialize working variables
            uint a = h[0];
            uint b = h[1];
            uint c = h[2];
            uint d = h[3];
            uint e = h[4];
            uint f = h[5];
            uint g = h[6];
            uint h0 = h[7];

            // Main loop
            for (int j = 0; j < 64; ++j)
            {
                uint S1 = RightRotate(e, 6) ^ RightRotate(e, 11) ^ RightRotate(e, 25);
                uint ch = (e & f) ^ (~e & g);
                uint temp1 = h0 + S1 + ch + K[j] + w[j];
                uint S0 = RightRotate(a, 2) ^ RightRotate(a, 13) ^ RightRotate(a, 22);
                uint maj = (a & b) ^ (a & c) ^ (b & c);
                uint temp2 = S0 + maj;

                h0 = g;
                g = f;
                f = e;
                e = d + temp1;
                d = c;
                c = b;
                b = a;
                a = temp1 + temp2;
            }

            // Update hash values
            h[0] += a;
            h[1] += b;
            h[2] += c;
            h[3] += d;
            h[4] += e;
            h[5] += f;
            h[6] += g;
            h[7] += h0;
        }

        // Convert hash value to bytes
        byte[] hash = new byte[32];
        for (int i = 0; i < 8; ++i)
            Array.Copy(BitConverter.GetBytes(h[i]), 0, hash, i * 4, 4);

        return hash;
    }

    private static byte[] PadData(byte[] data)
    {
        int originalLength = data.Length;
        int padLength = 64 - (originalLength + 9) % 64;
        byte[] paddedData = new byte[originalLength + padLength + 9];

        Array.Copy(data, paddedData, originalLength);
        paddedData[originalLength] = 0x80; // Add a single '1' bit

        // Add the message length
        ulong bitLength = (ulong)originalLength * 8;
        byte[] bitLengthBytes = BitConverter.GetBytes(bitLength);
        Array.Copy(bitLengthBytes, 0, paddedData, paddedData.Length - 8, 8);

        return paddedData;
    }

    private static uint RightRotate(uint value, int bits)
    {
        return (value >> bits) | (value << (32 - bits));
    }
}