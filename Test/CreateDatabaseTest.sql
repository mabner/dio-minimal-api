ALTER DATABASE CHARACTER SET latin1;


CREATE TABLE `__efmigrationshistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`MigrationId`)
) COLLATE=utf8mb4_uca1400_ai_ci;


CREATE TABLE `administrators` (
    `Id` int(11) NOT NULL AUTO_INCREMENT,
    `Email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci NOT NULL,
    `Password` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci NOT NULL,
    `Profile` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_uca1400_ai_ci;


CREATE TABLE `vehicles` (
    `Id` int(11) NOT NULL AUTO_INCREMENT,
    `Model` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci NOT NULL,
    `Make` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci NOT NULL,
    `Year` int(11) NOT NULL,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_uca1400_ai_ci;


