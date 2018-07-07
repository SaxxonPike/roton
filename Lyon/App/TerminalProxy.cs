using Roton.Core;

namespace Lyon.App
{
    public class TerminalProxy : ITerminal
    {
        private readonly IComposerProxy _composerProxy;

        public TerminalProxy(IComposerProxy composerProxy)
        {
            _composerProxy = composerProxy;
        }
        
        public void Clear() => 
            _composerProxy.SceneComposer?.Clear();

        public void Plot(int x, int y, AnsiChar ac) => 
            _composerProxy.SceneComposer?.Plot(x, y, ac);

        public AnsiChar Read(int x, int y) => 
            _composerProxy.SceneComposer?.Read(x, y) ?? new AnsiChar(0, 0);

        public void SetSize(int width, int height, bool wide) =>
            _composerProxy.SetScene(width, height, wide);

        public void Write(int x, int y, string value, int color) =>
            _composerProxy.SceneComposer?.Write(x, y, value, color);
    }
}