using UI.Menu;
using Zenject;

namespace Infrastructure.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPresenters();
        }

        private void BindPresenters()
        {
            Container.Bind<IGameMenuPresenter>().To<GameMenuPresenter>().AsSingle();
        }
    }
}