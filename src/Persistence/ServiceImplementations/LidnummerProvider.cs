using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.ServiceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.GtSWB.Persistence.ServiceImplementations;
internal class LidnummerProvider(IUnitOfWork unitOfWork) : ILidnummerProvider
{
    public int GetNext() => unitOfWork.AllLedenAggregate.MaxBy(lid => lid.Lidnummer)?.Lidnummer + 1 ?? 1;
}
