
#include "asservissement.h"
#include "UART_Protocol.h"
#include "Robot.h"
#include "ToolBox.h"


void SetupPidAsservissement(volatile PidCorrector* PidCorr, double Kp, double Ki, double Kd, double proportionelleMax, double integralMax,double deriveeMax){
PidCorr->Kp = Kp;
PidCorr->erreurProportionelleMax = proportionelleMax; //On limite la correction due au Kp
PidCorr->Ki = Ki;
PidCorr->erreurIntegraleMax = integralMax; //On limite la correction due au Ki
PidCorr->Kd = Kd;
PidCorr->erreurDeriveeMax = deriveeMax;


}

void PWMSetSpeedConsignePolaire(float vitesseLineaire, float vitesseAngulaire) {
robotState.vitesseDroiteConsigne = vitesseLineaire + vitesseAngulaire*(0.218/2);  
robotState.vitesseGaucheConsigne = vitesseAngulaire - vitesseAngulaire*(0.218/2);   
robotState.vitesseDroitePercent = -M_TO_PERCENT * robotState.vitesseDroiteConsigne;
robotState.vitesseGauchePercent = M_TO_PERCENT * robotState.vitesseGaucheConsigne;
LimitToInterval(robotState.vitesseDroitePercent , -100, 100);
LimitToInterval(robotState.vitesseGauchePercent , -100, 100);
}

//double Correcteur(volatile PidCorrector* PidCorr, double erreur)
//{
//PidCorr->erreur = erreur;
//double erreurProportionnelle = LimitToInterval(...);
//PidCorr->corrP = ...;
//PidCorr->erreurIntegrale += ...;
//PidCorr->erreurIntegrale = LimitToInterval(...);
//PidCorr->corrI = ...;
//double erreurDerivee = (erreur - PidCorr->epsilon_1)*FREQ_ECH_QEI;
//double deriveeBornee = LimitToInterval(erreurDerivee, -PidCorr->erreurDeriveeMax/PidCorr->Kd,
//PidCorr->erreurDeriveeMax/PidCorr->Kd);
//PidCorr->epsilon_1 = erreur;
//PidCorr->corrD = deriveeBornee * PidCorr->Kd;
//return PidCorr->corrP+PidCorr->corrI+PidCorr->corrD;
//}
//
//void UpdateAsservissement()
//{
//robotState.PidX.erreur = ...;
//robotState.PidTheta.erreur = ...;
//robotState.CorrectionVitesseLineaire =
//Correcteur(&robotState.PidX, robotState.PidX.erreur);
//robotState.CorrectionVitesseAngulaire = ...;
//PWMSetSpeedConsignePolaire(robotState.CorrectionVitesseLineaire,robotState.CorrectionVitesseAngulaire);
//}
