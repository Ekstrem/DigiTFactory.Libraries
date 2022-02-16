namespace Hive.SeedWorks.Definition
{
    /// <summary>
    /// Тип взаимодействия ограниченных контекстов.
    /// </summary>
    public enum InteractionOfBoundedContextsEnum
    {
        /// <summary>
        /// ОБЩЕЕ ЯДРО часто представляет собой СМЫСЛОВОЕ ЯДРО предметной области (CORE DOMAIN),
        /// набор ЕСТЕСТВЕННЫХ ПОДОБЛАСТЕЙ (GENERIC SUBDOMAINS) или то и другой отновременно.
        /// </summary>
        SharedKernel,
        
        /// <summary>
        /// Равные отношения взаимовлияния.
        /// </summary>
        Partners,
        
        /// <summary>
        /// Поставщик-потребитель.
        /// </summary>
        ClientSuppliers,
        
        /// <summary>
        /// Конформист.
        /// Расновидность отношения поставщик-потребитель, которая требует области подстройки.
        /// Возникает в том случае, если требования контекста потребителя не учитываются.
        /// </summary>
        Conformist,
        
        /// <summary>
        /// Предохранительный уровень.
        /// Разновидность отношения конформист, у которой область подстройки выделена в отдельный слой,
        /// с полноценной трансляцией для соблюдения правил ограниченного контекста.
        /// </summary>
        AntiCorruptionLayer,
        
        /// <summary>
        /// Служба с открытым протоколом.
        /// </summary>
        OpenHostService,
        
        /// <summary>
        /// Общедоступный язык.
        /// </summary>
        PublishedLanguage,
        
        /// <summary>
        /// Отдельное существование.
        /// </summary>
        SeparateWays
    }
}