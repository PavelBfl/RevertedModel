using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevertedModel
{
	/// <summary>
	/// Поставщик токенов сдвига команд
	/// </summary>
	public interface ITrackTokenProvider
	{
		/// <summary>
		/// Создать токен сдвига, токены должны создаваться угикальными
		/// </summary>
		/// <returns>Токен сдвига</returns>
		object CreateToken();
	}
}
