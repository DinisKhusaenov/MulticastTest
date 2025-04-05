using GameLogic.Gameplay.GameLogic;
using Gameplay.Clusters.Factory;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactories();
            BindGameLogicServices();
        }

        private void BindGameLogicServices()
        {
            Container.Bind<IClustersGenerator>().To<ClustersGenerator>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IClusterFactory>().To<ClusterFactory>().AsSingle();
            Container.Bind<IWordsContainerFactory>().To<WordsContainerFactory>().AsSingle();
        }
    }
}