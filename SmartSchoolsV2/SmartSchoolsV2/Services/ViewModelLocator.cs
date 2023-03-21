using SmartSchoolsV2.DataServices;
using SmartSchoolsV2.ViewModels;
using System;
using Unity;
using Unity.Lifetime;

namespace SmartSchoolsV2.Services
{
    public class ViewModelLocator
    {
        private readonly IUnityContainer _unityContainer;

        private static readonly ViewModelLocator _instance = new ViewModelLocator();

        public static ViewModelLocator Instance
        {
            get
            {
                return _instance;
            }
        }

        protected ViewModelLocator()
        {
            _unityContainer = new UnityContainer();

            // providers
            _unityContainer.RegisterType<IRequestProvider, RequestProvider>();

            // services
            _unityContainer.RegisterType<IChatService, ChatService>(new ContainerControlledLifetimeManager());

            // data services
            // ---

            // view models
            //_unityContainer.RegisterType<MainVM>();
            _unityContainer.RegisterType<ChatViewModel>();
            //_unityContainer.RegisterType<FacebookViewModel>();

        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public void Register<T>(T instance)
        {
            _unityContainer.RegisterInstance<T>(instance);
        }

        public void Register<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>();
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }
    }
}
