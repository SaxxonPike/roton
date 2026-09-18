using System;

namespace Torch.App.Impl;

public unsafe class TextureManager(SDL_Renderer* renderer) : ITextureManager
{
    public SDL_Texture* Create(int width, int height, SDL_TextureAccess access) =>
        SDL_CreateTexture(renderer, SDL_PIXELFORMAT_BGRA32, access, width, height);

    public void Destroy(SDL_Texture* texture) =>
        SDL_DestroyTexture(texture);

    public void Update(SDL_Texture* texture, void* pixels, int pitch) => 
        SDL_UpdateTexture(texture, null, (IntPtr)pixels, pitch);
}