namespace Torch.App;

public interface ITextureManager
{
    unsafe SDL_Texture* Create(int width, int height, SDL_TextureAccess access);
    unsafe void Destroy(SDL_Texture* texture);
    unsafe void Update(SDL_Texture* texture, void* pixels, int pitch);
}