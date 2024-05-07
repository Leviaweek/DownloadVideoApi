namespace VideoDownloaderApi.Abstractions;

public interface ICommand<in TCommand>: IBaseCommand;

public interface ICommand : IBaseCommand;