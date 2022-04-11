using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IMailService {
    Task Send(MailMessageModel request);
}
