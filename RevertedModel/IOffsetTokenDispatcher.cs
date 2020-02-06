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
	public interface IOffsetTokenDispatcher
	{
		/// <summary>
		/// Создать токен сдвига, каждый следующий предоставляемый токен должен быть больше преведущего
		/// </summary>
		/// <returns>Токен сдвига</returns>
		IComparable CreateToken();
	}
}
