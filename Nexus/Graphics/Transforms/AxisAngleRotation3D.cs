using System;

namespace Nexus.Graphics.Transforms
{
	public class AxisAngleRotation3D : Rotation
	{
        public Vector3D Axis
	    {
	        get;
	        set;
	    }

        public float Angle
		{
			get;
			set;
		}

		public AxisAngleRotation3D()
		{
			Axis = new Vector3D(0, 1, 0);
		    Angle = 0;
		}

	    public AxisAngleRotation3D(Vector3D axis, float angle)
	    {
            Axis = axis;
	        Angle = angle;
	    }

	    public AxisAngleRotation3D(Quaternion quaternion)
	    {
	        Axis = quaternion.Axis;
	        Angle = MathUtility.ToDegrees(quaternion.Angle);
	    }

		public override Quaternion Value => Quaternion.CreateFromAxisAngle(Axis, Angle);

	    public override string ToString()
	    {
	        return Axis.X + " " + Axis.Y + " " + Axis.Z + " " + Angle;
	    }
	}
}