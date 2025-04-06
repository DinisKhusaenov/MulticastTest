using GameLogic.Gameplay.GameLogic;
using Gameplay.Clusters.Factory;
using Gameplay.Levels;
using Infrastructure.Loading.Level;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactories();
            BindLevelServices();
        }

        private void BindLevelServices()
        {
            Container.Bind<ILevelDataLoader>().To<LevelDataLoader>().AsSingle();
            Container.Bind<ILevelSessionService>().To<LevelSessionService>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IClusterFactory>().To<ClusterFactory>().AsSingle();
            Container.Bind<IClustersContainerFactory>().To<ClustersContainerFactory>().AsSingle();
        }
    }
}