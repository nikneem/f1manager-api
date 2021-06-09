using System.Collections.Generic;
using System.Linq;
using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
    public static class NotificationMappings
    {

        public static List<NotificationDto> ToDtoList(this List<NotificationEntity> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }

        public static NotificationDto ToDto(this NotificationEntity entity)
        {
            var ntfType = NotificationType.All.FirstOrDefault(x => x.TypeIdentifier.Equals(entity.TypeIdentifier));
            if (ntfType == null)
            {
                throw new NotificationSystemException(NotificationErrorCode.UnknownTypeIdentifier,
                    $"The type {entity.TypeIdentifier} is not a known notification message type");
            }

            return new NotificationDto
            {
                IsRead = entity.IsRead,
                BodyTranslationKey = ntfType.BodyTranslationKey,
                Id = entity.Id,
                Substitutions = entity.Substitutions.ToDictionary(x=> x.Field, y=> y.Value),
                Severity = ntfType.Severity,
                TitleTranslationKey = ntfType.TitleTranslationKey,
                TypeIdentifier = entity.TypeIdentifier,
                ShowAsPopup = ntfType.ShowAsPopup,
                ShowInNotificationArea = ntfType.ShowInNotificationArea
            };
        }

    }
}
