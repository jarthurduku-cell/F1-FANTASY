-- ============================================================
--  F1 Fantasy League - Script de Base de Datos
--  Generado desde ERD: Fantasy F1 League BD
-- ============================================================

-- ============================================================
-- TABLAS INDEPENDIENTES (sin FK externas)
-- ============================================================

CREATE TABLE USER (
    user_id             INT             NOT NULL AUTO_INCREMENT,
    username            VARCHAR(100)    NOT NULL UNIQUE,
    email               VARCHAR(150)    NOT NULL UNIQUE,
    password_hash       VARCHAR(255)    NOT NULL,
    profile_picture_url VARCHAR(500),
    role                VARCHAR(50)     NOT NULL DEFAULT 'user',
    created_at          DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_user PRIMARY KEY (user_id)
);

CREATE TABLE CONSTRUCTOR (
    constructor_id  INT             NOT NULL AUTO_INCREMENT,
    name            VARCHAR(100)    NOT NULL,
    nationality     VARCHAR(100),
    CONSTRAINT pk_constructor PRIMARY KEY (constructor_id)
);

CREATE TABLE PILOT (
    pilot_id        INT             NOT NULL AUTO_INCREMENT,
    constructor_id  INT             NOT NULL,
    first_name      VARCHAR(100)    NOT NULL,
    last_name       VARCHAR(100)    NOT NULL,
    nationality     VARCHAR(100),
    code            VARCHAR(10),
    car_number      INT,
    active          BOOLEAN         NOT NULL DEFAULT TRUE,
    CONSTRAINT pk_pilot PRIMARY KEY (pilot_id),
    CONSTRAINT fk_pilot_constructor FOREIGN KEY (constructor_id)
        REFERENCES CONSTRUCTOR (constructor_id)
);

CREATE TABLE RACE (
    race_id             INT             NOT NULL AUTO_INCREMENT,
    season_year         INT             NOT NULL,
    round_number        INT             NOT NULL,
    grand_prix_name     VARCHAR(150)    NOT NULL,
    circuit_name        VARCHAR(150),
    race_date           DATETIME,
    prediction_deadline DATETIME,
    CONSTRAINT pk_race PRIMARY KEY (race_id)
);

-- ============================================================
-- LIGA Y MIEMBROS
-- ============================================================

CREATE TABLE LEAGUE (
    league_id       INT             NOT NULL AUTO_INCREMENT,
    name            VARCHAR(150)    NOT NULL,
    invite_code     VARCHAR(50)     UNIQUE,
    admin_user_id   INT             NOT NULL,
    starting_budget DECIMAL(15, 2)  NOT NULL DEFAULT 100.00,
    season_year     INT             NOT NULL,
    status          VARCHAR(50)     NOT NULL DEFAULT 'active',
    created_at      DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_league PRIMARY KEY (league_id),
    CONSTRAINT fk_league_admin FOREIGN KEY (admin_user_id)
        REFERENCES USER (user_id)
);

CREATE TABLE LEAGUEMEMBER (
    league_member_id    INT             NOT NULL AUTO_INCREMENT,
    league_id           INT             NOT NULL,
    user_id             INT             NOT NULL,
    remaining_budget    DECIMAL(15, 2)  NOT NULL,
    total_points        INT             NOT NULL DEFAULT 0,
    last_race_points    INT             NOT NULL DEFAULT 0,
    main_pilot_id       INT,
    joined_at           DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_leaguemember PRIMARY KEY (league_member_id),
    CONSTRAINT uq_leaguemember UNIQUE (league_id, user_id),
    CONSTRAINT fk_leaguemember_league FOREIGN KEY (league_id)
        REFERENCES LEAGUE (league_id),
    CONSTRAINT fk_leaguemember_user FOREIGN KEY (user_id)
        REFERENCES USER (user_id),
    CONSTRAINT fk_leaguemember_pilot FOREIGN KEY (main_pilot_id)
        REFERENCES PILOT (pilot_id)
);

-- ============================================================
-- MERCADO Y TRANSACCIONES
-- ============================================================

CREATE TABLE MARKETVALUE (
    market_value_id INT             NOT NULL AUTO_INCREMENT,
    pilot_id        INT             NOT NULL,
    race_id         INT             NOT NULL,
    value_amount    DECIMAL(15, 2)  NOT NULL,
    calculated_at   DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_marketvalue PRIMARY KEY (market_value_id),
    CONSTRAINT fk_marketvalue_pilot FOREIGN KEY (pilot_id)
        REFERENCES PILOT (pilot_id),
    CONSTRAINT fk_marketvalue_race FOREIGN KEY (race_id)
        REFERENCES RACE (race_id)
);

CREATE TABLE MARKETTRANSACTION (
    market_transaction_id   INT             NOT NULL AUTO_INCREMENT,
    league_id               INT             NOT NULL,
    buyer_member_id         INT             NOT NULL,
    seller_member_id        INT,
    pilot_id                INT             NOT NULL,
    transaction_type        VARCHAR(50)     NOT NULL,   -- 'buy', 'sell', 'transfer'
    amount                  DECIMAL(15, 2)  NOT NULL,
    created_at              DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_markettransaction PRIMARY KEY (market_transaction_id),
    CONSTRAINT fk_markettxn_league FOREIGN KEY (league_id)
        REFERENCES LEAGUE (league_id),
    CONSTRAINT fk_markettxn_buyer FOREIGN KEY (buyer_member_id)
        REFERENCES LEAGUEMEMBER (league_member_id),
    CONSTRAINT fk_markettxn_seller FOREIGN KEY (seller_member_id)
        REFERENCES LEAGUEMEMBER (league_member_id),
    CONSTRAINT fk_markettxn_pilot FOREIGN KEY (pilot_id)
        REFERENCES PILOT (pilot_id)
);

-- ============================================================
-- SQUAD / PLANTILLA
-- ============================================================

CREATE TABLE SQUADPILOT (
    squad_pilot_id          INT             NOT NULL AUTO_INCREMENT,
    league_member_id        INT             NOT NULL,
    pilot_id                INT             NOT NULL,
    acquisition_price       DECIMAL(15, 2)  NOT NULL,
    clause_value            DECIMAL(15, 2),
    is_protected            BOOLEAN         NOT NULL DEFAULT FALSE,
    protection_until_race_id INT,
    acquired_at             DATETIME        NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_squadpilot PRIMARY KEY (squad_pilot_id),
    CONSTRAINT fk_squadpilot_member FOREIGN KEY (league_member_id)
        REFERENCES LEAGUEMEMBER (league_member_id),
    CONSTRAINT fk_squadpilot_pilot FOREIGN KEY (pilot_id)
        REFERENCES PILOT (pilot_id),
    CONSTRAINT fk_squadpilot_race FOREIGN KEY (protection_until_race_id)
        REFERENCES RACE (race_id)
);

-- ============================================================
-- RESULTADOS DE CARRERA
-- ============================================================

CREATE TABLE RACERESULT (
    race_result_id      INT             NOT NULL AUTO_INCREMENT,
    race_id             INT             NOT NULL,
    pilot_id            INT             NOT NULL,
    finishing_position  INT,
    points_awarded      INT             NOT NULL DEFAULT 0,
    status              VARCHAR(50),    -- 'finished', 'dnf', 'dsq', etc.
    CONSTRAINT pk_raceresult PRIMARY KEY (race_result_id),
    CONSTRAINT uq_raceresult UNIQUE (race_id, pilot_id),
    CONSTRAINT fk_raceresult_race FOREIGN KEY (race_id)
        REFERENCES RACE (race_id),
    CONSTRAINT fk_raceresult_pilot FOREIGN KEY (pilot_id)
        REFERENCES PILOT (pilot_id)
);

-- ============================================================
-- PREDICCIONES SEMANALES (por carrera)
-- ============================================================

CREATE TABLE WEEKLYPREDICTION (
    weekly_prediction_id    INT         NOT NULL AUTO_INCREMENT,
    league_member_id        INT         NOT NULL,
    race_id                 INT         NOT NULL,
    submitted_at            DATETIME,
    total_score             INT         NOT NULL DEFAULT 0,
    doubled_score           INT         NOT NULL DEFAULT 0,
    is_locked               BOOLEAN     NOT NULL DEFAULT FALSE,
    CONSTRAINT pk_weeklyprediction PRIMARY KEY (weekly_prediction_id),
    CONSTRAINT uq_weeklyprediction UNIQUE (league_member_id, race_id),
    CONSTRAINT fk_weeklypred_member FOREIGN KEY (league_member_id)
        REFERENCES LEAGUEMEMBER (league_member_id),
    CONSTRAINT fk_weeklypred_race FOREIGN KEY (race_id)
        REFERENCES RACE (race_id)
);

CREATE TABLE PREDICTIONDETAIL (
    prediction_detail_id    INT     NOT NULL AUTO_INCREMENT,
    weekly_prediction_id    INT     NOT NULL,
    pilot_id                INT     NOT NULL,
    predicted_position      INT     NOT NULL,
    actual_position         INT,
    points_earned           INT     NOT NULL DEFAULT 0,
    CONSTRAINT pk_predictiondetail PRIMARY KEY (prediction_detail_id),
    CONSTRAINT fk_preddetail_prediction FOREIGN KEY (weekly_prediction_id)
        REFERENCES WEEKLYPREDICTION (weekly_prediction_id),
    CONSTRAINT fk_preddetail_pilot FOREIGN KEY (pilot_id)
        REFERENCES PILOT (pilot_id)
);

-- ============================================================
-- PREDICCIONES DE TEMPORADA
-- ============================================================

CREATE TABLE SEASONPREDICTION (
    season_prediction_id    INT             NOT NULL AUTO_INCREMENT,
    league_member_id        INT             NOT NULL,
    submitted_at            DATETIME,
    drivers_score           INT             NOT NULL DEFAULT 0,
    constructors_score      INT             NOT NULL DEFAULT 0,
    total_score             INT             NOT NULL DEFAULT 0,
    CONSTRAINT pk_seasonprediction PRIMARY KEY (season_prediction_id),
    CONSTRAINT fk_seasonpred_member FOREIGN KEY (league_member_id)
        REFERENCES LEAGUEMEMBER (league_member_id)
);

CREATE TABLE SEASONPREDICTIONDRIVER (
    season_prediction_driver_id INT     NOT NULL AUTO_INCREMENT,
    season_prediction_id        INT     NOT NULL,
    predicted_rank              INT     NOT NULL,
    pilot_id                    INT     NOT NULL,
    CONSTRAINT pk_seasonpreddriver PRIMARY KEY (season_prediction_driver_id),
    CONSTRAINT fk_seasonpreddriver_prediction FOREIGN KEY (season_prediction_id)
        REFERENCES SEASONPREDICTION (season_prediction_id),
    CONSTRAINT fk_seasonpreddriver_pilot FOREIGN KEY (pilot_id)
        REFERENCES PILOT (pilot_id)
);

CREATE TABLE SEASONPREDICTIONCONSTRUCTOR (
    season_prediction_constructor_id    INT     NOT NULL AUTO_INCREMENT,
    season_prediction_id                INT     NOT NULL,
    predicted_rank                      INT     NOT NULL,
    constructor_id                      INT     NOT NULL,
    CONSTRAINT pk_seasonpredconstructor PRIMARY KEY (season_prediction_constructor_id),
    CONSTRAINT fk_seasonpredctor_prediction FOREIGN KEY (season_prediction_id)
        REFERENCES SEASONPREDICTION (season_prediction_id),
    CONSTRAINT fk_seasonpredctor_constructor FOREIGN KEY (constructor_id)
        REFERENCES CONSTRUCTOR (constructor_id)
);

-- ============================================================
-- ÍNDICES ADICIONALES PARA RENDIMIENTO
-- ============================================================

CREATE INDEX idx_pilot_constructor     ON PILOT (constructor_id);
CREATE INDEX idx_leaguemember_league   ON LEAGUEMEMBER (league_id);
CREATE INDEX idx_leaguemember_user     ON LEAGUEMEMBER (user_id);
CREATE INDEX idx_squadpilot_member     ON SQUADPILOT (league_member_id);
CREATE INDEX idx_squadpilot_pilot      ON SQUADPILOT (pilot_id);
CREATE INDEX idx_marketvalue_pilot     ON MARKETVALUE (pilot_id);
CREATE INDEX idx_marketvalue_race      ON MARKETVALUE (race_id);
CREATE INDEX idx_markettxn_league      ON MARKETTRANSACTION (league_id);
CREATE INDEX idx_markettxn_buyer       ON MARKETTRANSACTION (buyer_member_id);
CREATE INDEX idx_raceresult_race       ON RACERESULT (race_id);
CREATE INDEX idx_raceresult_pilot      ON RACERESULT (pilot_id);
CREATE INDEX idx_weeklypred_member     ON WEEKLYPREDICTION (league_member_id);
CREATE INDEX idx_weeklypred_race       ON WEEKLYPREDICTION (race_id);
CREATE INDEX idx_preddetail_pred       ON PREDICTIONDETAIL (weekly_prediction_id);
CREATE INDEX idx_seasonpred_member     ON SEASONPREDICTION (league_member_id);
CREATE INDEX idx_seasonpreddrv_pred    ON SEASONPREDICTIONDRIVER (season_prediction_id);
CREATE INDEX idx_seasonpredctor_pred   ON SEASONPREDICTIONCONSTRUCTOR (season_prediction_id);
