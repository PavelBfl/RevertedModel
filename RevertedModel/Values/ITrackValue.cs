namespace RevertedModel
{
	public interface ITrackValue<T> : ITrackObject
	{
		T Value { get; set; }
	}
}