#ifndef ROBOT_H
#define ROBOT_H
typedef struct robotStateBITS {
union {
struct {
unsigned char taskEnCours;
float vitesseGaucheConsigne;
float vitesseGaucheCommandeCourante;
float vitesseDroiteConsigne;
float vitesseDroiteCommandeCourante;
float distanceTelemetreDroit1;
float distanceTelemetreDroit;
float distanceTelemetreCentre;
float distanceTelemetreGauche1;
float distanceTelemetreGauche;
double vitesseDroitFromOdometry;
double vitesseGaucheFromOdometry;
double vitesseLineaireFromOdometry;
double vitesseAngulaireFromOdometry;
double xPosFromOdometry_1;
double xPosFromOdometry;
double yPosFromOdometry_1;
double yPosFromOdometry;
double angleRadianFromOdometry_1;
double angleRadianFromOdometry;

};
};
} ROBOT_STATE_BITS;
extern volatile ROBOT_STATE_BITS robotState;

#endif /* ROBOT_H */
