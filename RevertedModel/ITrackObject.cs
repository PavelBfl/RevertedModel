namespace RevertedModel
{
	/// <summary>
	/// Интерфейс отслеживаемого объекта
	/// </summary>
	public interface ITrackObject
	{
		/// <summary>
		/// Диспетчер отслеживания изменений
		/// </summary>
		TrackDispatcher TrackDispatcher { get; }
	}
}