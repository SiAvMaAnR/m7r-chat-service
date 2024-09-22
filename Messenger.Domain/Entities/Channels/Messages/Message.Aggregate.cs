﻿using Messenger.Domain.Entities.Accounts;
using Messenger.Domain.Entities.Attachments;

namespace Messenger.Domain.Entities.Messages;

public partial class Message : IAggregateRoot
{
    public void AddChildMessage(Message message)
    {
        ChildMessages.Add(message);
    }

    public void AddReadAccounts(Account account)
    {
        ReadAccounts.Add(account);
    }

    public void AddAttachment(Attachment attachment)
    {
        Attachments.Add(attachment);
    }

    public void AddAttachments(List<Attachment> attachments)
    {
        attachments.ForEach(
            (attachment) =>
            {
                if (attachment.MessageId == null)
                {
                    Attachments.Add(attachment);
                }
            }
        );
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void Read()
    {
        IsRead = true;
    }
}
