using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
public class RhCapNative
{

     
        [DllImport("share_mem_client_dll")]
        static extern void NAPI_share_mem_send(int fage,int w, int h, int c, IntPtr imgdata);
        [DllImport("share_mem_client_dll")]
        static extern int NAPI_share_mem_init();
        [DllImport("share_mem_client_dll")]
        static extern void NAPI_share_mem_Close();

        public void SetTextureToShareMem(Texture2D tex)
        {
            GCHandle pixelHandle = GCHandle.Alloc(tex.GetPixels32(), GCHandleType.Pinned);
            IntPtr pixelPtr = pixelHandle.AddrOfPinnedObject();
            NAPI_share_mem_send(1,tex.width,tex.height,4, pixelPtr);
            pixelHandle.Free();
		    UnityEngine.Object.Destroy(tex);
        }
        public void StopShare()
        {
            NAPI_share_mem_Close();
        }

        public int InitShare()
        {
           return NAPI_share_mem_init();
        }

    private static RhCapNative uniqueInstance;
    public static RhCapNative GetInstance()
    {
        if (uniqueInstance == null)
        {
            uniqueInstance = new RhCapNative();
        }
        return uniqueInstance;
    }

}
